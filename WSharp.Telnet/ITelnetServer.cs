using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace WSharp.Telnet
{
    public interface ITelnetServer<T> : IDisposable
        where T : ITelnetConnection
    {
        bool IsRunning { get; }

        ObservableCollection<T> Connections { get; }

        Task StartAsync(CancellationToken cancellationToken = default);

        void Stop();
        Task DisposeConnectionsAsync();

        event SentReceivedEventHandler ReceivedAsync;
        event SentReceivedEventHandler SentAsync;
        event EventHandler<bool> StateChanged;
    }
}
