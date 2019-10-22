using System.Collections.ObjectModel;

namespace WSharp.Logging.Loggers
{
    /// <summary>Logger that logs to memory (a collection of logs)</summary>
    public interface IMemoryLogger : ILogger
    {
        /// <summary>Collection of logs that have been logged.</summary>
        ReadOnlyObservableCollection<ILogEntry> Logs { get; }
    }
}