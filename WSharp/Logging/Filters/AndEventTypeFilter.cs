using System.Diagnostics;
using System.Linq;

namespace WSharp.Logging.Filters
{
    /// <summary>
    ///     An <see cref="ILogFilter"/> that only accepts a log when all the event types in
    ///     <see cref="EventTypes"/> are present in the log.
    /// </summary>
    public class AndEventTypeFilter : ILogFilter
    {
        /// <summary>Event types a log needs to have to pass.</summary>
        public TraceEventType[] EventTypes { get; set; }

        /// <summary>Check whether a log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        public bool FilterLog(ILogEntry log) => EventTypes.All(x => (log.EventType & x) == x);
    }
}