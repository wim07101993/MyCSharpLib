using System.Net;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public delegate Task<string> ReceivedTelnetMessageEventHandler(IPAddress client, string message);
}
