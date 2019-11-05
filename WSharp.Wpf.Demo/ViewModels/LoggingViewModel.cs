using WSharp.Logging;
using WSharp.Logging.Loggers;
using WSharp.Wpf.Demo.ViewModelInterfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Prism.Events;

namespace WSharp.Wpf.Demo.ViewModels
{
    public class LoggingViewModel : AViewModel, ILoggingViewModel
    {
        private readonly IMemoryLogger _memoryLogger;

        public LoggingViewModel(IEventAggregator eventAggregator, ILogger logger, IMemoryLogger memoryLogger) 
            : base(eventAggregator, logger)
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
