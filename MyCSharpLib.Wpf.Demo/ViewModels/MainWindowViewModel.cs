using MyCSharpLib.Services.Logging.Loggers;
using MyCSharpLib.Wpf.Demo.Strings;
using MyCSharpLib.Wpf.Demo.ViewModelInterfaces;

namespace MyCSharpLib.Wpf.Demo.ViewModels
{
    public class MainWindowViewModel : AViewModel, IMainWindowViewModel
    {
        public MainWindowViewModel(ApplicationStrings strings, ILogDispatcher logger,
            ILoggingViewModel loggingViewModel) 
            : base(strings, logger)
        {
            LoggingViewModel = loggingViewModel;
        }

        public ILoggingViewModel LoggingViewModel { get; }
    }
}
