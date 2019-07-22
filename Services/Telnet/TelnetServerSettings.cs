using Prism.Mvvm;

namespace MyCSharpLib.Services.Telnet
{
    public class TelnetServerSettings : BindableBase
    {
        private int _portNumber = 3500;

        public int PortNumber
        {
            get => _portNumber;
            set => SetProperty(ref _portNumber, value);
        }
    }
}
