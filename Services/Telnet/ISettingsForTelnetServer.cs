namespace MyCSharpLib.Services.Telnet
{
    public interface ISettingsForTelnetServer : ISettings
    {
        TelnetServerSettings TelnetServerSettings { get; set; }
    }
}
