using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public interface ITelnetServer
    {
        bool IsListening { get; }

        ObservableCollection<TelnetServerConnection> Connections { get; }

        Task ListenAndServeAsync(CancellationToken cancellationToken = default);

        void StopListeningAndServing();

        event ReceivedTelnetMessageEventHandler ReceivedMessage;
    }
}
