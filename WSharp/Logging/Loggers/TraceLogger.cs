using System.Diagnostics;

namespace WSharp.Logging.Loggers
{
    /// <summary>Logger that logs to the <see cref="Trace"/></summary>
    public class TraceLogger : ALogger, ITraceLogger
    {
        /// <summary>Constructs a new instance of the <see cref="TraceLogger"/>.</summary>
        public TraceLogger()
        {
        }

        /// <summary>Method that actually logs the log entry.</summary>
        /// <param name="logEntry">Entry to log.</param>
        protected override void InternalLog(ILogEntry logEntry) => Trace.Write(logEntry);
    }
}
