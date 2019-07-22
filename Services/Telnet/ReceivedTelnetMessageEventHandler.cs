using System.Net;

namespace MyCSharpLib.Services.Telnet
{
    public delegate string ReceivedTelnetMessageEventHandler(IPAddress client, string gmessage);
}
