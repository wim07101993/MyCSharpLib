using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public class TelnetServerConnection : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public Guid Id { get; set; }
        public TcpClient Client { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public void Dispose() => _ = DisposeAsync();

        public async Task DisposeAsync()
        {
            CancellationTokenSource.Cancel();
            await Task.Delay(1000);
            Client.Dispose();
            IsDisposed = true;
        }

        public async Task TryDisposeAsync()
        {
            try
            {
                await DisposeAsync();
            }
            catch
            {
            }
        }
    }
}
