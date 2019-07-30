using Prism.Mvvm;

namespace MyCSharpLib.Services.Telnet
{
    public interface ITelnetServerSettings : ISettings
    {
        int TelnetPortNumber { get; set; }
    }
}
