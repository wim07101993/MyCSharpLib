using System.Collections.ObjectModel;

namespace WSharp.Logging.Loggers
{
    /// <summary>Logger that logs to memory (a collection of logs)</summary>
    public class MemoryLogger : ALogger, IMemoryLogger
    {
        #region PROPERTIES

        /// <summary>Modifyable collection of logs that have been logged.</summary>
        protected ObservableCollection<ILogEntry> InternalLogs { get; } = new ObservableCollection<ILogEntry>();

        /// <summary>Collection of logs that have been logged.</summary>
        public ReadOnlyObservableCollection<ILogEntry> Logs => new ReadOnlyObservableCollection<ILogEntry>(InternalLogs);

        #endregion PROPERTIES

        #region METHODS

        /// <summary>Clears the log collection.</summary>
        /// <param name="isDisposing">Indicates whether the logger is disposing.</param>
        public override void Dispose(bool isDisposing)
        {
            if (isDisposing)
                return;

            InternalLogs.Clear();
        }

        /// <summary>Method that actually logs the log entry.</summary>
        /// <param name="logEntry">Entry to log.</param>
        protected override void InternalLog(ILogEntry logEntry) => InternalLogs.Add(logEntry);

        #endregion METHODS
    }
}
