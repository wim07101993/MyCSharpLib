using MyCSharpLib.Services.Serialization;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public interface ITelnetConnection : INotifyPropertyChanged, IDisposable
    {
        ISerializerDeserializer SerializerDeserializer { get; }

        Guid Id { get; }

        TcpClient TcpClient { get; }
        string RemoteHost { get; }

        CancellationToken CancellationToken { get; }

        bool IsDisposed { get; }
        bool IsListening { get; }


        Task StartListeningAsync(CancellationToken cancellationToken = default);
        void StopListening();

        Task WriteAsync(string value);
        Task WriteLineAsync(string value);
        Task WriteAsync(object value, ISerializer serializer = null);
        Task WriteAsync(byte[] buffer, int offset, int count);


        event ReceivedAsyncEventHandler ReceivedAsync;
    }
}
