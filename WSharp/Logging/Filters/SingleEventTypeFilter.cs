using System.Diagnostics;

namespace WSharp.Logging.Filters
{
    /// <summary>Onnly lets one <see cref="TraceEventType"/> pass.</summary>
    public class SingleEventTypeFilter : ILogFilter
    {
        /// <summary>Event type to pass.</summary>
        public TraceEventType EventType { get; set; }

        /// <summary>Check whether a log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        public bool FilterLog(ILogEntry log) => (log.EventType & EventType) == EventType;
    }
}