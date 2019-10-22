using System.Diagnostics;

namespace WSharp.Logging.Filters
{
    /// <summary>Onnly lets one <see cref="TraceEventType"/> pass.</summary>
    public class SingleEventTypeFilter : ALogFilter
    {
        public SingleEventTypeFilter(TraceEventType eventType)
        {
            EventType = eventType;
        }


        /// <summary>Event type to pass.</summary>
        protected TraceEventType EventType { get; }

        /// <summary>Check whether a log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        public override bool CanLog(ILogEntry log) => (log.EventType & EventType) == EventType;
    }
}