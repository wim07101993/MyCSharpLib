using System.Collections.ObjectModel;

namespace WSharp.Logging.Loggers
{
    public class MemoryLogger : ALogger, IMemoryLogger
    {
        #region FIELDS

        private ObservableCollection<ILogEntry> _logs = new ObservableCollection<ILogEntry>();

        #endregion FIELDS

        #region PROPERTIES

        public ReadOnlyObservableCollection<ILogEntry> Logs => new ReadOnlyObservableCollection<ILogEntry>(_logs);

        #endregion PROPERTIES

        #region METHODS

        public override void Dispose(bool isDisposing)
        {
            if (isDisposing)
                return;

            _logs.Clear();
        }

        public override void InternalLog(ILogEntry logEntry) => _logs.Add(logEntry);

        #endregion METHODS
    }
}