using WSharp.Services.Logging;
using WSharp.Services.Logging.Loggers;
using WSharp.Wpf.Demo.ViewModelInterfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace WSharp.Wpf.Demo.ViewModels
{
    public class LoggingViewModel : AViewModel, ILoggingViewModel
    {
        private readonly IMemoryLogger _memoryLogger;

        public LoggingViewModel(ILogDispatcher logger, IMemoryLogger memoryLogger) 
            : base(logger)
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
