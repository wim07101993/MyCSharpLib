using System.Threading.Tasks;

namespace WSharp.Telnet
{
    public delegate Task SentReceivedEventHandler(ITelnetConnection connection, SentReceivedEventArgs args);
}
