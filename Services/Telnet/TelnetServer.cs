using MyCSharpLib.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public class TelnetServer : ITelnetServer
    {
        #region FIELDS

        private readonly ISettingsForTelnetServer _settings;

        private readonly object _lock = new object();
        private readonly List<TcpClient> _clients = new List<TcpClient>();
        private readonly List<NetworkStream> _streams = new List<NetworkStream>();

        private CancellationTokenSource _cancellationTokenSource;

        #endregion FIELDS


        #region CONSTRUCTOR

        public TelnetServer(ISettingsForTelnetServer settings)
        {
            _settings = settings;
        }

        #endregion CONSTRUCTOR


        #region METHODS

        public Task ListenAndServeAsync()
            => Task.Factory.StartNew(() =>
        {
            var listener = new TcpListener(IPAddress.Any, _settings.TelnetServerSettings.PortNumber);
            listener.Start();

            _cancellationTokenSource = new CancellationTokenSource();

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var client = listener.AcceptTcpClient();
                    lock (_lock)
                    {
                        _clients.Add(client);
                        _ = HandleClientAsync(client, _cancellationTokenSource.Token);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
        });

        public async Task StopListeningAndServingAsync()
        {
            _cancellationTokenSource.Cancel();

            await Task.Delay(1000);

            lock (_lock)
            {
                foreach (var stream in _streams)
                    stream.TryDispose();
                foreach (var client in _clients)
                    client.TryDispose();

                _clients.Clear();
                _streams.Clear();
            }
        }

        public async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            NetworkStream stream = null;
            try
            {
                using (client)
                {
                    stream = client.GetStream();
                    using (stream)
                    {
                        _streams.Add(stream);
                        var ipAdress = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                        var i = 0;
                        var buffer = new byte[256];

                        do
                        {
                            // read the message
                            i = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                            var receivedMessage = Encoding.ASCII.GetString(buffer, 0, i);

                            // fire event
                            var returnMessage = ReceivedMessage?.Invoke(ipAdress, receivedMessage);
                            if (string.IsNullOrEmpty(returnMessage))
                                continue;

                            // return message to client
                            var returnBytes = returnMessage.ToAsciiBytes();
                            await stream.WriteAsync(returnBytes, 0, returnBytes.Length, cancellationToken);
                        }
                        while (i != 0 && !cancellationToken.IsCancellationRequested);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            finally
            {
                client.TryDispose();
                stream?.TryDispose();
            }

            _clients.Remove(client);
            _streams.Remove(stream);
        }

        #endregion METHODS


        #region EVENTS

        public event ReceivedTelnetMessageEventHandler ReceivedMessage;

        #endregion EVENTS
    }
}
