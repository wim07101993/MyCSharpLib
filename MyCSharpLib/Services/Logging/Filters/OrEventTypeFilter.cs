using System.Diagnostics;
using System.Linq;

namespace MyCSharpLib.Services.Logging.Filters
{
    public class OrEventTypeFilter : ILogFilter
    {
        public TraceEventType[] EventTypes { get; set; }

        public bool FilterLog(ILogEntry log) => EventTypes.Any(x => (log.EventType & x) == x);
    }
}
