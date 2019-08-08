using MyCSharpLib.Services.Logging;
using MyCSharpLib.Services.Logging.Loggers;
using MyCSharpLib.Wpf.Demo.Strings;
using MyCSharpLib.Wpf.Demo.ViewModelInterfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MyCSharpLib.Wpf.Demo.ViewModels
{
    public class LoggingViewModel : AViewModel, ILoggingViewModel
    {
        private readonly IMemoryLogger _memoryLogger;

        public LoggingViewModel(ApplicationStrings strings, ILogDispatcher logger, IMemoryLogger memoryLogger) 
            : base(strings, logger)
        {
            _memoryLogger = memoryLogger;
            _memoryLogger.Log(nameof(LoggingViewModel), "Hello world, this is a test");
            _memoryLogger.Log(
                source: nameof(LoggingViewModel), 
                payload: new[] { "Hello world", "this is a test" }, 
                eventType: TraceEventType.Information, 
                title: "test");
        }

        public ReadOnlyObservableCollection<ILogEntry> Logs => _memoryLogger.Logs;
    }
}
