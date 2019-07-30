﻿using MyCSharpLib.Extensions;
using MyCSharpLib.Services.Serialization;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public abstract class ATelnetServer<T> : ITelnetServer<T>
       where T : ITelnetConnection
    {
        #region FIELDS

        private bool _isRunning;
        private bool _isAccepting;

        #endregion FIELDS


        #region CONSTRUCTOR

        protected ATelnetServer(ITelnetServerSettings settings, ISerializerDeserializer serializerDeserializer)
        {
            Settings = settings;
            SerializerDeserializer = serializerDeserializer;

            Connections = new ObservableCollection<T>();
            Connections.CollectionChanged += OnClientsCollectionChanged;
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        protected object Lock { get; } = new object();

        protected ITelnetServerSettings Settings { get; }
        protected ISerializerDeserializer SerializerDeserializer { get; }
        protected CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (Equals(_isRunning, value))
                    return;
                _isRunning = value;
                StateChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        /// A collection that holds all active connections.
        /// If a new connection is added, it is automatically listened to.
        /// When a connection is removed, it is disposed.
        /// If one of the connections throws an exception while trying to read or write, it is also automatically removed.
        /// </summary>
        public ObservableCollection<T> Connections { get; }

        #endregion PROPERTIES


        #region METHODS

        /// <summary>
        /// Starts listening to any ip address on the given port number (settings).
        /// If a new listener comes in, it is added to the connections collection.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task StartAsync(CancellationToken cancellationToken = default)
            => Task.Factory.StartNew(() =>
            {
                var listener = new TcpListener(IPAddress.Any, Settings.TelnetPortNumber);
                lock (Lock)
                {
                    if (IsRunning)
                        throw new NotSupportedException("Cannot start listening twice");

                    Connections.Clear();
                    listener.Start();
                    IsRunning = true;
                }

                var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, CancellationTokenSource.Token);

                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), Tuple.Create(listener, cancellationTokenSource.Token));
                    _isAccepting = true;

                    while (!cancellationTokenSource.IsCancellationRequested && _isAccepting) { }
                }

                lock (Lock)
                {
                    IsRunning = false;
                    listener.Stop();
                }
            });

        protected void DoAcceptTcpClientCallback(IAsyncResult asyncResult)
        {
            lock (Lock)
            {
                if (!IsRunning)
                    return;

                var tuple = (Tuple<TcpListener, CancellationToken>)asyncResult.AsyncState;
                var listener = tuple.Item1;
                var cancellationToken = tuple.Item2;

                var tcpClient = listener.EndAcceptTcpClient(asyncResult);

                Connections.Add(CreateNewConnection(tcpClient, cancellationToken));
                _isAccepting = false;
            }
        }

        public virtual void Stop()
        {
            lock (Lock)
            {
                Connections.Clear();
                CancellationTokenSource.Cancel();
            }
        }

        protected virtual void OnClientsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!IsRunning)
                return;

            var addedConnections = e.NewItems?.Cast<T>();
            var removedConnections = e.OldItems?.Cast<T>();

            if (addedConnections != null)
                foreach (var connection in addedConnections)
                    _ = HandleClientAsync(connection);

            if (removedConnections != null)
                foreach (var connection in removedConnections)
                    if (!connection.IsDisposed)
                        connection.TryDispose();
        }

        protected virtual async Task HandleClientAsync(T connection)
        {
            connection.ReceivedAsync += RaiseReceivedAsync;
            try
            {
                await connection.StartListeningAsync(CancellationTokenSource.Token);
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

        protected abstract T CreateNewConnection(TcpClient tcpClient, CancellationToken cancellationToken);

        protected virtual Task RaiseReceivedAsync(ITelnetConnection connection, ReceivedEventArgs args)
            => ReceivedMessage?.Invoke(connection, args);

        #endregion METHODS


        #region EVENTS

        public event ReceivedAsyncEventHandler ReceivedMessage;
        public event EventHandler<bool> StateChanged;

        #endregion EVENTS
    }
}
