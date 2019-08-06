using System.Collections.ObjectModel;
using MyCSharpLib.Services.Logging;
using MyCSharpLib.Services.Logging.Loggers;
using MyCSharpLib.Wpf.Demo.Strings;
using MyCSharpLib.Wpf.Demo.ViewModelInterfaces;

namespace MyCSharpLib.Wpf.Demo.ViewModels
{
    public class LoggingViewModel : AViewModel, ILoggingViewModel
    {
        private readonly IMemoryLogger _logger;


        public LoggingViewModel(ApplicationStrings strings, ILogDispatcher logDispatcher, IMemoryLogger logger) : base(strings, logDispatcher)
        {
            _logger = logger;

            Logger.Log(nameof(LoggingViewModel), "Created loggin view model");
        }


        public ReadOnlyObservableCollection<ILogEntry> Entries => _logger.Logs;
    }
}
