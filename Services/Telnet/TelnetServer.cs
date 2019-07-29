using MyCSharpLib.Extensions;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;

namespace MyCSharpLib.Services.Telnet
{
    public class TelnetServer : ITelnetServer
    {
        #region FIELDS

        private readonly ITelnetServerSettings _settings;

        private readonly object _lock = new object();

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        #endregion FIELDS


        #region CONSTRUCTOR

        public TelnetServer(ITelnetServerSettings settings)
        {
            _settings = settings;

            Connections = new ObservableCollection<TelnetServerConnection>();
            Connections.CollectionChanged += OnClientsCollectionChanged;
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        public bool IsListening { get; private set; }

        /// <summary>
        /// A collection that holds all active connections.
        /// If a new connection is added, it is automatically listened to.
        /// When a connection is removed, it is disposed.
        /// If one of the connections throws an exception while trying to read or write, it is also automatically removed.
        /// </summary>
        public ObservableCollection<TelnetServerConnection> Connections { get; }

        #endregion PROPERTIES


        #region METHODS

        /// <summary>
        /// Starts listening to any ip address on the given port number (settings).
        /// If a new listener comes in, it is added to the connections collection.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ListenAndServeAsync(CancellationToken cancellationToken = default)
            => Task.Factory.StartNew(() =>
        {
            var listener = new TcpListener(IPAddress.Any, _settings.PortNumber);
            listener.Start();

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);

            IsListening = true;
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {   lock (_lock)
                    {
                        Connections.Add(new TelnetServerConnection
                        {
                            Id = Guid.NewGuid(),
                            Client = listener.AcceptTcpClient(),
                            CancellationTokenSource = _cancellationTokenSource
                        });
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    IsListening = false;
                    throw;
                }
            }
            IsListening = false;
        });
        
        public void StopListeningAndServing()
        {
            lock (_lock)
            {
                Connections.Clear();
            }
        }

        private void OnClientsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var addedConnections = e.NewItems?.Cast<TelnetServerConnection>();
            var removedConnections = e.OldItems?.Cast<TelnetServerConnection>();

            if (addedConnections != null)
                foreach (var connection in addedConnections)
                    _ = HandleClientAsync(connection);

            if (removedConnections != null)
                foreach (var connection in removedConnections)
                    if (!connection.IsDisposed)
                        connection.Client.TryDispose();
        }

        private async Task HandleClientAsync(TelnetServerConnection connection)
        {
            NetworkStream stream = null;

            try
            {
                using (connection.Client)
                {
                    stream = connection.Client.GetStream();
                    using (stream)
                    {
                        var ipAdress = ((IPEndPoint)connection.Client.Client.RemoteEndPoint).Address;
                        var i = 0;
                        var buffer = new byte[256];

                        do
                        {
                            // read the message
                            i = await stream.ReadAsync(buffer, 0, buffer.Length, connection.CancellationTokenSource.Token);
                            var receivedMessage = Encoding.ASCII.GetString(buffer, 0, i);

                            // fire event
                            var returnMessage = await ReceivedMessage?.Invoke(ipAdress, receivedMessage);
                            if (string.IsNullOrEmpty(returnMessage))
                                continue;

                            // return message to client
                            var returnBytes = returnMessage.ToAsciiBytes();
                            await stream.WriteAsync(returnBytes, 0, returnBytes.Length, connection.CancellationTokenSource.Token);
                        }
                        while (i != 0 && !connection.CancellationTokenSource.IsCancellationRequested);
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
                Connections.RemoveFirst(x => x.Id == connection.Id);
            }

            Connections.RemoveFirst(x => x.Id == connection.Id);
        }
        
        #endregion METHODS


        #region EVENTS

        public event ReceivedTelnetMessageEventHandler ReceivedMessage;

        #endregion EVENTS
    }
}
