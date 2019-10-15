using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WSharp.Logging.Loggers
{
    public class LogDispatcher : ALogger, ICollection<ILogger>, ILogDispatcher
    {
        #region FIELDS

        private Collection<ILogger> _loggers = new Collection<ILogger>();

        #endregion FIELDS

        #region PROPERTIES

        public int Count => _loggers.Count;

        public bool IsReadOnly => false;

        #endregion PROPERTIES

        #region METHODS

        public override void Dispose(bool isDisposing)
        {
            foreach (var l in this)
                l.Dispose(isDisposing);
        }

        public override void InternalLog(ILogEntry logEntry)
        {
            foreach (var l in this)
                l.Log(logEntry);
        }

        public void Add(ILogger item) => _loggers.Add(item);

        public void Clear() => _loggers.Clear();

        public bool Contains(ILogger item) => _loggers.Contains(item);

        public void CopyTo(ILogger[] array, int arrayIndex) => _loggers.CopyTo(array, arrayIndex);

        public bool Remove(ILogger item) => _loggers.Remove(item);

        public IEnumerator<ILogger> GetEnumerator() => _loggers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _loggers.GetEnumerator();

        #endregion METHODS
    }
}