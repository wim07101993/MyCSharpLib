using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public interface ITelnetServer
    {
        Task ListenAndServeAsync();
        Task StopListeningAndServingAsync();

        event ReceivedTelnetMessageEventHandler ReceivedMessage;
    }
}
