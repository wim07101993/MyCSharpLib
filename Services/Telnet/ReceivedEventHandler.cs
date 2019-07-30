using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public delegate Task ReceivedAsyncEventHandler(ITelnetConnection connection, ReceivedEventArgs args);
}
