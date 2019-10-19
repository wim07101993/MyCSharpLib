using WSharp.Serialization;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace WSharp.Telnet
{
    public interface ITelnetConnection : INotifyPropertyChanged, IDisposable
    {
        ISerializerDeserializer SerializerDeserializer { get; }

        Guid Id { get; }

        TcpClient TcpClient { get; }
        string RemoteHost { get; }
        string LocalHost { get; }

        bool IsDisposed { get; }
        bool IsListening { get; }
        

        Task StartListeningAsync(CancellationToken cancellationToken = default);
        void StopListening();

        Task WriteAsync(string value, CancellationToken cancellationToken = default);
        Task WriteLineAsync(string value, CancellationToken cancellationToken = default);
        Task WriteAsync(object value, ISerializer serializer = null, CancellationToken cancellationToken = default);
        Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default);

        void StopAllTransactions();

        event SentReceivedEventHandler ReceivedAsync;
        event SentReceivedEventHandler SentAsync;
    }
}
