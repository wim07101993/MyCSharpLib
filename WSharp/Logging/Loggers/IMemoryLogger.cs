using System.Collections.ObjectModel;

namespace WSharp.Logging.Loggers
{
    public interface IMemoryLogger : ILogger
    {
        ReadOnlyObservableCollection<ILogEntry> Logs { get; }
    }
}