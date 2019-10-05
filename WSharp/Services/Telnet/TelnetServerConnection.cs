using WSharp.Extensions;
using WSharp.Services.Logging.Loggers;
using WSharp.Services.Serialization;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WSharp.Services.Telnet
{
    public class TelnetServerConnection : BindableBase, ITelnetConnection
    {
        #region FIELDS

        private const int BufferSize = 256;
        private const int AreYouThereInterval = 1000;

        private readonly object _lock = new object();

        private readonly CancellationTokenSource _checkAliveCancelTokenSource = new CancellationTokenSource();
        private CancellationTokenSource _receiveCancelTokenSource = new CancellationTokenSource();
        private CancellationTokenSource _sendCancelTokenSource = new CancellationTokenSource();
        
        private bool _isDisposed;
        private bool _isDisposing;
        private bool _isListening;

        #endregion FIELDS


        #region CONSTRUCTOR

        public TelnetServerConnection(ILogger logger, ISerializerDeserializer serializerDeserializer, TcpClient tcpClient)
        {
            Logger = logger;
            SerializerDeserializer = serializerDeserializer;
            Id = Guid.NewGuid();

            TcpClient = tcpClient;

            _ = DisposeOnDisconnectAsync(_checkAliveCancelTokenSource.Token);

            Logger.Log(ClassName, $"Created new telnetServerConnection with {RemoteHost}. Id = {Id}");
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        protected virtual string ClassName => nameof(TelnetServerConnection);

        protected ILogger Logger { get; }
        public ISerializerDeserializer SerializerDeserializer { get; }

        public Guid Id { get; }

        public TcpClient TcpClient { get; }

        public string RemoteHost 
            => IsDisposed 
                ? null 
                : (TcpClient?.Client.RemoteEndPoint as IPEndPoint)?.ToString();
        public string LocalHost => IsDisposed
                ? null
                : (TcpClient?.Client.LocalEndPoint as IPEndPoint)?.ToString();

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

        public void StopAllTransactions()
        {
            _receiveCancelTokenSource?.Cancel();
            _sendCancelTokenSource?.Cancel();

            _receiveCancelTokenSource.Dispose();
            _sendCancelTokenSource.Dispose();
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (_isDisposing)
                    return;
                _isDisposing = true;

                var remoteHost = RemoteHost;
                Logger.Log(ClassName, new[] { $"Disposing connection to {remoteHost}.", $"({Id})" });

                StopAllTransactions();

                if (IsDisposed)
                {
                    Logger.Log(ClassName, new[] { $"Connection already disposed.", $"({Id})" });
                    return;
                }

                _checkAliveCancelTokenSource.Cancel();
                _checkAliveCancelTokenSource.Dispose();

                TcpClient.Dispose();

                IsDisposed = true;
                _isDisposing = false;

                Logger.Log( nameof(TelnetServerConnection), new[] { $"Disposed connection to {remoteHost}.", $"({Id})" });
            }
        }

        #region reading

        public async Task StartListeningAsync(CancellationToken cancellationToken = default)
        {
            lock (_lock)
            {
                if (IsListening)
                    throw new NotSupportedException("Cannot start listening twice");
                IsListening = true;
            }
            
            var remoteHost = RemoteHost;
            _receiveCancelTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            
            Logger.Log(ClassName, new[] { $"Started listening to {remoteHost}.", $"({Id})" });
            await ListenAsync(_receiveCancelTokenSource.Token);
            Logger.Log(ClassName, new[] { $"Stopped listening to {remoteHost}.", $"({Id})" });

            lock (_lock)
            {
                IsListening = false;
            }
        }

        public void StopListening()
        {
            lock (_lock)
            {
                if (!IsListening)
                    return;

                Logger.Log(ClassName, new[] { $"Stopping to listen to {RemoteHost}.", $"({Id})" });
                _receiveCancelTokenSource.Cancel();
            }
        }

        private Task ListenAsync(CancellationToken cancellationToken)
            => Task.Factory.StartNew(() =>
        {
            var remoteHost = RemoteHost;
            var readState = new ReadState();
            while (!cancellationToken.IsCancellationRequested && IsListening)
            {
                readState.IsReading = true;
                try
                {
                    TcpClient.GetStream().BeginRead(readState.Buffer, 0, readState.Buffer.Length, EndReadCallback, readState);
                }
                catch (IOException)
                {
                    Logger.Log(ClassName, $"Client at {remoteHost} closed connection");
                }
                

                while (readState.IsReading)
                {
                    if (cancellationToken.IsCancellationRequested || !IsListening)
                        return;
                }

                if (readState.IsMessageComplete)
                {
                    _ = RaiseReceivedAsync(readState.CompleteMessage);
                    readState = new ReadState();
                }
            }
        });

        private void EndReadCallback(IAsyncResult asyncResult)
        {
            if (!IsListening)
                return;

            try
            {
                var i = TcpClient.GetStream().EndRead(asyncResult);

                var readState = (ReadState)asyncResult.AsyncState;
                if (i <= 0)
                {
                    readState.IsReading = false;
                    return;
                }

                var buffer = readState.Buffer;
                var message = readState.CompleteMessage;

                var bytes = new byte[i];
                Array.Copy(buffer, 0, bytes, 0, i);
                message.AddRange(bytes);

                if (bytes.Length >= 2 && bytes[i - 2] == '\r' && bytes[i - 1] == '\n')
                    readState.IsMessageComplete = true;
                else
                    readState.IsMessageComplete = false;

                readState.IsReading = false;
            }
            catch (InvalidOperationException)
            {
                if (!IsDisposed)
                    throw;
                return;
            }
        }

        protected virtual async Task RaiseReceivedAsync(IEnumerable<byte> bytes)
        {
            var args = new SentReceivedEventArgs(bytes, SerializerDeserializer);

            Logger.Log(ClassName, new object[] 
            {
                $"Received content from {RemoteHost}.",
                $"({Id})",
                args.BytesContent,
                "As ASCII:",
                args.StringContent
            });
            
            await ReceivedAsync?.Invoke(this, args);
        }

        #endregion reading


        #region writing

        public async Task WriteAsync(string value, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(value))
                return;

            var bytes = value.ToAsciiBytes();
            await WriteAsync(bytes, 0, bytes.Length, cancellationToken);
        }

        public async Task WriteLineAsync(string value, CancellationToken cancellationToken = default) 
            => await WriteAsync($"{value ?? string.Empty}\r\n", cancellationToken);

        public async Task WriteAsync(object value, ISerializer serializer = null, CancellationToken cancellationToken = default)
        {
            if (serializer == null)
                serializer = SerializerDeserializer;

            var serialized = await serializer.SerializeAsync(value);
            await WriteLineAsync(serialized, cancellationToken);
        }

        public Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
            => Task.Factory.StartNew(() =>
        {
            if (_sendCancelTokenSource?.IsCancellationRequested != false)
                _sendCancelTokenSource = new CancellationTokenSource();

            var sendCancelTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _sendCancelTokenSource.Token);

            Logger.Log(ClassName, new object[] 
            {
                $"Writeing bytes to {RemoteHost}.",
                $"({Id})",
                buffer,
                "As ASCII",
                Encoding.ASCII.GetString(buffer)
            });

            var sendState = new SendState { IsSending = true };
            TcpClient.GetStream().BeginWrite(buffer, 0, buffer.Length, EndWriteCallBack, sendState);

            while (sendState.IsSending)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;
            }

            byte[] bufferCopy = new byte[count - offset];
            Array.Copy(buffer, offset, bufferCopy, 0, count);
            _ = RaiseSentAsync(bufferCopy);
        });

        private void EndWriteCallBack(IAsyncResult asyncResult)
        {
            var sendState = (SendState)asyncResult.AsyncState;

            TcpClient.GetStream().EndWrite(asyncResult);
            sendState.IsSending = false;

        }

        protected virtual async Task RaiseSentAsync(IEnumerable<byte> bytes)
        {
            var args = new SentReceivedEventArgs(bytes, SerializerDeserializer);

            Logger.Log(ClassName, new object[]
            {
                $"Received content from {RemoteHost}.",
                $"({Id})",
                args.BytesContent,
                "As ASCII:",
                args.StringContent
            });

            await SentAsync?.Invoke(this, args);
        }

        #endregion writing


        #region check alive

        protected async Task DisposeOnDisconnectAsync(CancellationToken cancellationToken)
        {
            while (await CheckAliveAsync())
            {
                try
                {
                    await Task.Delay(AreYouThereInterval, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    Logger.Log(ClassName, "Stopped the server => Task.Delay threw error in DisposeOnDisconnectAsync");
                    return;
                }
            }

            Logger.Log(ClassName, new[] { $"Client disconnected.", $"({Id})" });
            Dispose();
        }

        protected async Task<bool> CheckAliveAsync()
        {
            if (!TcpClient.Connected)
                return false;

            //if (_isSending)
            //{
            //    while (_isSending) { }
            //    return TcpClient.Connected;
            //}

            await TcpClient.GetStream().WriteAsync(new byte[1], 0, 1);

            return TcpClient.Connected;
        }

        #endregion check alive

        #endregion METHODS


        #region EVENTS

        public event SentReceivedEventHandler ReceivedAsync;
        public event SentReceivedEventHandler SentAsync;

        #endregion EVENTS


        #region CLASSES

        private class ReadState
        {
            public ReadState()
            {
                Buffer = new byte[BufferSize];
                CompleteMessage = new List<byte>();
            }


            public byte[] Buffer { get; }
            public List<byte> CompleteMessage { get; }

            public bool IsMessageComplete { get; set; }
            public bool IsReading { get; set; }
        }

        private class SendState
        {
            public bool IsSending { get; set; }
        }

        #endregion CLASSES
    }
}
