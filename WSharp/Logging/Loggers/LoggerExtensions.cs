using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WSharp.Logging.Loggers
{
    public static class LoggerExtensions
    {
        /// <summary>Logs the given entry if it passes the logfilter.</summary>
        /// <param name="logEntry">Log entry to log.</param>
        public static void Log(this ILogger logger, IBufferLogEntry logEntry) 
            => logger.Log(new LogEntry(logEntry));

        /// <summary>
        ///     Builds a log entry from the given parameters and logs it if it passes the filter.
        /// </summary>
        /// <param name="source">The source that logs.</param>
        /// <param name="payload">The payload to log.</param>
        /// <param name="eventType">Type of the event that causes the log.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        public static void Log(this ILogger logger, string source, object payload, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null)
            => logger.Log(new LogEntry(source, tag, payload: payload == null ? null : new[] { payload }, eventType: eventType));

        /// <summary>
        ///     Builds a log entry from the given parameters and logs it if it passes the filter.
        /// </summary>
        /// <param name="source">The source that logs.</param>
        /// <param name="payload">The payload to log.</param>
        /// <param name="eventType">Type of the event that causes the log.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        public static void Log(this ILogger logger, string source, object[] payload, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null)
            => logger.Log(new LogEntry(source, tag, payload: payload, eventType: eventType));

        /// <summary>
        ///     Builds a log entry from the given parameters and logs it if it passes the filter.
        /// </summary>
        /// <param name="eventType">Type of the event that causes the log.</param>
        /// <param name="source">The source that logs.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        /// <param name="title">Title of the log</param>
        /// <param name="payload">The payload to log.</param>
        /// <param name="traceOptions">Tracing options that should be added to the log</param>
        /// <param name="indentLevel">Level of indent at which the log should be logged.</param>
        /// <param name="indentSize">Size of the indents.</param>
        public static void Log(
            this ILogger logger,
            TraceEventType eventType = TraceEventType.Verbose,
            string source = null,
            [CallerMemberName] string tag = null,
            string title = null,
            IList<object> payload = null,
            TraceOptions traceOptions = TraceOptions.DateTime,
            ushort indentLevel = 0,
            ushort indentSize = 0)
            => logger.Log(new LogEntry(
               source,
               tag,
               eventType,
               title,
               payload == null ? null : new ReadOnlyCollection<object>(payload),
               traceOptions,
               indentLevel,
               indentSize));

        /// <summary>Logs the given entry if it passes the logfilter.</summary>
        /// <param name="logEntry">Log entry to log.</param>
        public static Task LogAsync(this ILogger logger, IBufferLogEntry logEntry)
            => logger.LogAsync(new LogEntry(logEntry));

        /// <summary>
        ///     Builds a log entry from the given parameters and logs it if it passes the filter.
        /// </summary>
        /// <param name="source">The source that logs.</param>
        /// <param name="payload">The payload to log.</param>
        /// <param name="eventType">Type of the event that causes the log.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        public static Task LogAsync(this ILogger logger, string source, object payload, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null)
            => logger.LogAsync(new LogEntry(source, tag, payload: payload == null ? null : new[] { payload }, eventType: eventType));

        /// <summary>
        ///     Builds a log entry from the given parameters and logs it if it passes the filter.
        /// </summary>
        /// <param name="source">The source that logs.</param>
        /// <param name="payload">The payload to log.</param>
        /// <param name="eventType">Type of the event that causes the log.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        public static Task LogAsync(this ILogger logger, string source, object[] payload, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null)
            => logger.LogAsync(new LogEntry(source, tag, payload: payload, eventType: eventType));

        /// <summary>
        ///     Builds a log entry from the given parameters and logs it if it passes the filter.
        /// </summary>
        /// <param name="eventType">Type of the event that causes the log.</param>
        /// <param name="source">The source that logs.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        /// <param name="title">Title of the log</param>
        /// <param name="payload">The payload to log.</param>
        /// <param name="traceOptions">Tracing options that should be added to the log</param>
        /// <param name="indentLevel">Level of indent at which the log should be logged.</param>
        /// <param name="indentSize">Size of the indents.</param>
        public static Task LogAsync(
            this ILogger logger,
            TraceEventType eventType = TraceEventType.Verbose,
            string source = null,
            [CallerMemberName] string tag = null,
            string title = null,
            IList<object> payload = null,
            TraceOptions traceOptions = TraceOptions.DateTime,
            ushort indentLevel = 0,
            ushort indentSize = 0)
            => logger.LogAsync(new LogEntry(
               source,
               tag,
               eventType,
               title,
               payload == null ? null : new ReadOnlyCollection<object>(payload),
               traceOptions,
               indentLevel,
               indentSize));
    }
}
