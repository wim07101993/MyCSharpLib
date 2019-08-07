using MyCSharpLib.Services.Logging;
using MyCSharpLib.Services.Logging.Loggers;
using MyCSharpLib.Wpf.Demo.Strings;
using MyCSharpLib.Wpf.Demo.ViewModelInterfaces;
using System.Collections.ObjectModel;

namespace MyCSharpLib.Wpf.Demo.ViewModels
{
    public class LoggingViewModel : AViewModel, ILoggingViewModel
    {
        private readonly IMemoryLogger _memoryLogger;

        public LoggingViewModel(ApplicationStrings strings, ILogDispatcher logger, IMemoryLogger memoryLogger) 
            : base(strings, logger)
        {
            _memoryLogger = memoryLogger;
        }

        public ReadOnlyObservableCollection<ILogEntry> Logs => _memoryLogger.Logs;
    }
}
