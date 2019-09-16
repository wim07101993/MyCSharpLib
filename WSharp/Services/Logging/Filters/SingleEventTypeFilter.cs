using System.Diagnostics;

namespace WSharp.Services.Logging.Filters
{
    public class SingleEventTypeFilter : ILogFilter
    {
        public TraceEventType EventType { get; set; }

        public bool FilterLog(ILogEntry log) => (log.EventType & EventType) == EventType;
    }
}
