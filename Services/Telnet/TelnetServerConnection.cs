using MyCSharpLib.Extensions;
using MyCSharpLib.Services.Serialization;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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
        private bool _isListening;
        
        #endregion FIELDS


        #region CONSTRUCTOR

        public TelnetServerConnection(TcpClient tcpClient, CancellationToken cancellationToken, ISerializerDeserializer serializerDeserializer)
        {
            SerializerDeserializer = serializerDeserializer;
            Id = Guid.NewGuid();

            TcpClient = tcpClient;
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        public ISerializerDeserializer SerializerDeserializer { get; }

        public Guid Id { get; }

        public TcpClient TcpClient { get; }
        public string RemoteHost => (TcpClient?.Client.RemoteEndPoint as IPEndPoint)?.ToString();

        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public bool IsDisposed
        {
            get => _isDisposed;
            private set => SetProperty(ref _isDisposed, value);
        }
        
        public bool IsListening
        {
            get => _isListening;
            private set
            {
                lock (_lock)
                    SetProperty(ref _isListening, value);
            }
        }

        #endregion PROPERTIES


        #region METHDOS

        public void Dispose() => _ = DisposeAsync();

        public async Task DisposeAsync()
        {
            StopListening();
            await Task.Delay(10);
            TcpClient.Dispose();
            _cancellationTokenSource.Dispose();
            IsDisposed = true;
        }

        public async Task TryDisposeAsync()
        {
            try
            {
                await DisposeAsync();
            }
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
            {
                // TODO
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

            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, CancellationToken);
            cancellationTokenSource.Token.Register(() => TcpClient.Close());

            _ = DisposeOnDisconnectAsync();

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                var i = await TcpClient.GetStream().ReadAsync(_buffer, 0, _buffer.Length, cancellationTokenSource.Token);

                if (i <= 0)
                    continue;

                var bytes = new byte[i];
                Array.Copy(_buffer, 0, bytes, 0, i);

                _ = RaiseReceivedAsync(bytes);
            }
        }

        protected async Task DisposeOnDisconnectAsync()
        {
            while (await CheckAliveAsync())
            {
                await Task.Delay(1000, CancellationToken);
            }

            await DisposeAsync();
        }

        protected async Task<bool> CheckAliveAsync()
        {
            await TcpClient.GetStream().WriteAsync(new byte[1], 0, 1);
            return TcpClient.Connected;
        }

        public void StopListening()
        {
            _cancellationTokenSource.Cancel();
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

        public async Task WriteAsync(byte[] buffer, int offset, int count) => await TcpClient.GetStream().WriteAsync(buffer, offset, count, CancellationToken);

        #endregion PROPERTIES


        #region METHODS

        public virtual async Task RaiseReceivedAsync(byte[] bytes)
        {
            var args = new ReceivedEventArgs(bytes, SerializerDeserializer);

#if DEBUG
            Debug.WriteLine($"Received: {args.StringContent}");
#endif

            await ReceivedAsync?.Invoke(this, args);
        }

        #endregion METHODS


        #region EVENTS

        public event ReceivedAsyncEventHandler ReceivedAsync;

        #endregion EVENTS
    }
}
