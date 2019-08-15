using MyCSharpLib.Services.Logging.Loggers;
using MyCSharpLib.Wpf.Demo.Strings;
using MyCSharpLib.Wpf.Demo.ViewModelInterfaces;

namespace MyCSharpLib.Wpf.Demo.ViewModels
{
    public class MainWindowViewModel : AViewModel, IMainWindowViewModel
    {
        public MainWindowViewModel(ApplicationStrings strings, ILogDispatcher logger,
            ILoggingViewModel loggingViewModel, ObjectBrowserViewModel objectBrowserViewModel) 
            : base(strings, logger)
        {
            LoggingViewModel = loggingViewModel;
            ObjectBrowserViewModel = objectBrowserViewModel;
        }

        public ILoggingViewModel LoggingViewModel { get; }
        public ObjectBrowserViewModel ObjectBrowserViewModel { get; }
    }
}
