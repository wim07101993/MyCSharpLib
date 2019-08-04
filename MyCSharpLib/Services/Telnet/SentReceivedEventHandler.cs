using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public delegate Task SentReceivedEventHandler(ITelnetConnection connection, SentReceivedEventArgs args);
}
