using System.Collections.ObjectModel;

namespace WSharp.Services.Logging.Loggers
{
    public interface IMemoryLogger : ILogger
    {
        ReadOnlyObservableCollection<ILogEntry> Logs { get; }
    }
}
