using System.Threading.Tasks;

namespace WSharp.Services.Telnet
{
    public delegate Task SentReceivedEventHandler(ITelnetConnection connection, SentReceivedEventArgs args);
}
