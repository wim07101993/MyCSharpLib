using MyCSharpLib.Services.Logging;
using System.Diagnostics;

namespace MyCSharpLib.Extensions
{
    public static class TraceSourceExtensions
    {
        public static void TraceEvent(this TraceSource traceSource, TraceEventType eventType = TraceEventType.Verbose, int id = 0, string message = null) 
            => traceSource.TraceEvent(eventType, id, message);

        public static void TraceEntry(this TraceSource traceSource, LogEntry entry, int id = 0)
        {
        }
    }
}
