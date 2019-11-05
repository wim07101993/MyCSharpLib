using Prism.Events;
using WSharp.Logging.Loggers;
using WSharp.Wpf.Demo.ViewModelInterfaces;

namespace WSharp.Wpf.Demo.ViewModels
{
    public class MainWindowViewModel : AViewModel, IMainWindowViewModel
    {
        public MainWindowViewModel(IEventAggregator eventAggregator, ILogger logger,
            ILoggingViewModel loggingViewModel, ObjectBrowserViewModel objectBrowserViewModel) 
            : base(eventAggregator, logger)
        {
            LoggingViewModel = loggingViewModel;
            ObjectBrowserViewModel = objectBrowserViewModel;
        }

        public ILoggingViewModel LoggingViewModel { get; }
        public ObjectBrowserViewModel ObjectBrowserViewModel { get; }
    }
}
