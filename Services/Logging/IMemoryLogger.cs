using System.Collections.ObjectModel;

namespace MyCSharpLib.Services.Logging.Loggers
{
    public interface IMemoryLogger : ILogger
    {
        ReadOnlyObservableCollection<ILogEntry> Logs { get; }
    }
}
