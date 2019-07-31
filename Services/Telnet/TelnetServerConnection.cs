using MyCSharpLib.Extensions;
using MyCSharpLib.Services.Logging;
using MyCSharpLib.Services.Serialization;
using Prism.Mvvm;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public class TelnetServerConnection : BindableBase, ITelnetConnection
    {
        #region FIELDS

        private const int BufferSize = 256;
        private const int AreYouThereInterval = 1000;

        private readonly object _lock = new object();
        private readonly byte[] _buffer = new byte[BufferSize];

        private CancellationTokenSource _cancellationTokenSource;
        private bool _isDisposed;
        private bool _isDisposing;
        private bool _isListening;
        
        #endregion FIELDS


        #region CONSTRUCTOR

        public TelnetServerConnection(TcpClient tcpClient, CancellationToken cancellationToken, ISerializerDeserializer serializerDeserializer)
        {
            SerializerDeserializer = serializerDeserializer;
            Id = Guid.NewGuid();

            TcpClient = tcpClient;
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Trace.WriteLines($"Created new telnetServerConnection with {RemoteHost}. Id = {Id}");
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        public ISerializerDeserializer SerializerDeserializer { get; }

        public Guid Id { get; }

        public TcpClient TcpClient { get; }
        public string RemoteHost 
            => IsDisposed 
                ? null 
                : (TcpClient?.Client.RemoteEndPoint as IPEndPoint)?.ToString();

        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public bool IsDisposed
        {
            get => _isDisposed;
            private set => SetProperty(ref _isDisposed, value);
        }
        
        public bool IsListening
        {
            get => _isListening;
            private set => SetProperty(ref _isListening, value);
        }

        #endregion PROPERTIES


        #region METHDOS

        public void Dispose()
        {
            lock (_lock)
            {
                if (_isDisposing)
                    return;

                _isDisposing = true;

                var remoteHost = RemoteHost;
                Trace.WriteLines($"Disposing connection to {remoteHost}.", $"({Id})");

                if (IsListening)
                    StopListening();

                if (IsDisposed)
                {
                    Trace.WriteLines($"Connection already disposed.", $"({Id})");
                    return;
                }

                TcpClient.Dispose();
                _cancellationTokenSource.Dispose();

                IsDisposed = true;
                _isDisposing = false;

                Trace.WriteLines($"Disposed connection to {remoteHost}.", $"({Id})");
            }
        }
        
        public async Task StartListeningAsync(CancellationToken cancellationToken = default)
        {
            lock (_lock)
            {
                if (IsListening)
                    throw new NotSupportedException("Cannot start listening twice");
                IsListening = true;
            }

            var remoteHost = RemoteHost;
            Trace.WriteLines($"Started listening to {remoteHost}.", $"({Id})");

            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, CancellationToken);
            //cancellationTokenSource.Token.Register(Dispose);

            _ = DisposeOnDisconnectAsync();

            while (!cancellationTokenSource.IsCancellationRequested && IsListening)
            {
                try
                {
                    var i = await TcpClient.GetStream().ReadAsync(_buffer, 0, _buffer.Length, cancellationTokenSource.Token);

                    if (i <= 0)
                        continue;

                    var bytes = new byte[i];
                    Array.Copy(_buffer, 0, bytes, 0, i);

                    _ = RaiseReceivedAsync(bytes);
                }
                catch (ObjectDisposedException)
                {
                    if (cancellationTokenSource.IsCancellationRequested)
                        Trace.WriteLines("Disposed the tcp client (server probalby stopped), nothing to worry about");
                    else
                        throw;
                }
                catch(IOException)
                {
                    lock(_lock)
                    {
                        Trace.WriteLines("There was an error while trying to read from the connection. Disposing the connection.");
                        if (!IsDisposed)
                            Dispose();
                    }
                }
            }

            Trace.WriteLines($"Stopped listening to {remoteHost}.", $"({Id})");

            lock (_lock)
            {
                IsListening = false;
            }
        }

        protected async Task DisposeOnDisconnectAsync()
        {
            while (await CheckAliveAsync())
            {
                try
                {
                    await Task.Delay(AreYouThereInterval, CancellationToken);
                }
                catch (TaskCanceledException)
                {
                    Trace.WriteLineIndented("Stopped the server => Task.Delay threw error in DisposeOnDisconnectAsync");
                    return;
                }
            }

            Trace.WriteLines($"Client disconnected.", $"({Id})");
            Dispose();
        }

        protected async Task<bool> CheckAliveAsync()
        {
            if (!TcpClient.Connected)
                return false;

            await TcpClient.GetStream().WriteAsync(new byte[1], 0, 1);

            return TcpClient.Connected;
        }

        public void StopListening()
        {
            lock (_lock)
            {
                if (!IsListening)
                    return;
                
                Trace.WriteLines($"Stopping to listen to {RemoteHost}.", $"({Id})");
                _cancellationTokenSource.Cancel();
                
                IsListening = false;
            }
        }

        public async Task WriteAsync(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            var bytes = value.ToAsciiBytes();
            await WriteAsync(bytes, 0, bytes.Length);
        }

        public async Task WriteLineAsync(string value) => await WriteAsync($"{value ?? string.Empty}\r\n");

        public async Task WriteAsync(object value, ISerializer serializer = null)
        {
            if (serializer == null)
                serializer = SerializerDeserializer;

            var serialized = await serializer.SerializeAsync(value);
            await WriteLineAsync(serialized);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count)
        {
            Trace.WriteLines($"Writeing bytes to {RemoteHost}.", 
                $"({Id})", 
                buffer, 
                "As ASCII", 
                Encoding.ASCII.GetString(buffer));

            await TcpClient.GetStream().WriteAsync(buffer, offset, count, CancellationToken);
        }

        #endregion PROPERTIES


        #region METHODS

        public virtual async Task RaiseReceivedAsync(byte[] bytes)
        {
            var args = new ReceivedEventArgs(bytes, SerializerDeserializer);

            Trace.WriteLines($"Received content from {RemoteHost}.", 
                $"({Id})", 
                args.BytesContent, 
                "As ASCII:", 
                args.StringContent);

            await ReceivedAsync?.Invoke(this, args);
        }

        #endregion METHODS


        #region EVENTS

        public event ReceivedAsyncEventHandler ReceivedAsync;

        #endregion EVENTS
    }
}
