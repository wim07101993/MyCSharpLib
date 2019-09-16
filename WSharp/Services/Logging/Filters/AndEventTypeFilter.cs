using System.Diagnostics;
using System.Linq;

namespace WSharp.Services.Logging.Filters
{
    public class AndEventTypeFilter : ILogFilter
    {
        public TraceEventType[] EventTypes { get; set; }

        public bool FilterLog(ILogEntry log) => EventTypes.All(x => (log.EventType & x) == x);
    }
}
