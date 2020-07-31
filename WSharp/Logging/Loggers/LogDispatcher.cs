using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WSharp.Logging.Loggers
{
    /// <summary>A logger that dispatches logs to other loggers.</summary>
    public class LogDispatcher : ALogger, ICollection<ILogger>, ILogDispatcher
    {
        #region FIELDS

        private readonly Collection<ILogger> _loggers = new Collection<ILogger>();

        #endregion FIELDS

        #region PROPERTIES

        /// <summary>Count of the loggers.</summary>
        public int Count => _loggers.Count;

        /// <summary>This collection is not readonly.</summary>
        public bool IsReadOnly => false;

        #endregion PROPERTIES

        #region METHODS

        /// <summary>Disposes al the loggers in the dispatcher.</summary>
        /// <param name="isDisposing">Indicates whether the logger is disposing.</param>
        public override void Dispose(bool isDisposing)
        {
            foreach (var l in this)
                l.Dispose(isDisposing);
        }

        /// <summary>Method that actually logs the log entry.</summary>
        /// <param name="logEntry">Entry to log.</param>
        protected override void InternalLog(ILogEntry logEntry)
        {
            foreach (var l in this)
                l.Log(logEntry);
        }

        protected override Task InternalLogAsync(ILogEntry logEntry)
            => Task.WhenAll(this.Select(x => x.LogAsync(logEntry)));

        /// <summary>Adds a logger to the dispatchers collection.</summary>
        /// <param name="item">Logger to add to the dispatcher</param>
        public void Add(ILogger item) => _loggers.Add(item);

        /// <summary>Removes all loggers from the dispatchers collection.</summary>
        public void Clear() => _loggers.Clear();

        /// <summary>Checks whether the dispatchers collection contains a logger.</summary>
        /// <param name="item">Logger that is searched for.</param>
        /// <returns>Whether the dispatchers collection contains a logger.</returns>
        public bool Contains(ILogger item) => _loggers.Contains(item);

        /// <summary>Copies the dispatchers collection to another array.</summary>
        /// <param name="array">Array to copy to.</param>
        /// <param name="arrayIndex">Index to insert the copied items at.</param>
        public void CopyTo(ILogger[] array, int arrayIndex) => _loggers.CopyTo(array, arrayIndex);

        /// <summary>Removes a logger from the dispatchers collection.</summary>
        /// <param name="item">Logger to remove from the dispatchers collection.</param>
        /// <returns>Whether the logger has been removed.</returns>
        public bool Remove(ILogger item) => _loggers.Remove(item);

        /// <summary>Returns the enumerator to enumerate over.</summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<ILogger> GetEnumerator() => _loggers.GetEnumerator();

        /// <summary>Returns the enumerator to enumerate over.</summary>
        /// <returns>An enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator() => _loggers.GetEnumerator();

        #endregion METHODS
    }
}
