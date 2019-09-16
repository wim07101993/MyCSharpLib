using WSharp.Services.Logging.Loggers;
using WSharp.Wpf.Demo.ViewModelInterfaces;

namespace WSharp.Wpf.Demo.ViewModels
{
    public class MainWindowViewModel : AViewModel, IMainWindowViewModel
    {
        public MainWindowViewModel(ILogDispatcher logger,
            ILoggingViewModel loggingViewModel, ObjectBrowserViewModel objectBrowserViewModel) 
            : base(logger)
        {
            LoggingViewModel = loggingViewModel;
            ObjectBrowserViewModel = objectBrowserViewModel;
        }

        public ILoggingViewModel LoggingViewModel { get; }
        public ObjectBrowserViewModel ObjectBrowserViewModel { get; }
    }
}
