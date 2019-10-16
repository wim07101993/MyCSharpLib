using System.Diagnostics;

namespace WSharp.Logging.Loggers
{
    public class TraceLogger : ALogger, ITraceLogger
    {
        public TraceLogger()
        {
        }

        public override void InternalLog(ILogEntry logEntry) => Trace.Write(logEntry);
    }
}
