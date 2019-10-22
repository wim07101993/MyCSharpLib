using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WSharp.Logging.Loggers
{
    /// <summary>Interface that describes a logger.</summary>
    public interface ILogger : IDisposable
    {
        /// <summary>A lock to synchronize tasks/threads.</summary>
        object GlobalLock { get; }

        /// <summary>If <see langword="false"/>: no logs are logged.</summary>
        bool ShouldLog { get; set; }

        /// <summary>Filter that filters what logs should be logged.</summary>
        ILogFilter Filter { get; set; }

        /// <summary>
        ///     A buffer to write to and in the end log with the <see cref="Log(IBufferLogEntry)"/> or <see cref="LogBuffer()"/> method.
        /// </summary>
        IBufferLogEntry Buffer { get; }

        /// <summary>Logs the given entry if it passes the logfilter.</summary>
        /// <param name="logEntry">Log entry to log.</param>
        void Log(IBufferLogEntry logEntry);

        /// <summary>Logs the given entry if it passes the logfilter.</summary>
        /// <param name="logEntry">Log entry to log.``</param>
        void Log(ILogEntry logEntry);

        /// <summary>Logs the <see cref="Buffer"/>.</summary>
        void LogBuffer();

        /// <summary>
        ///     Builds a log entry from the given parameters and logs it if it passes the filter.
        /// </summary>
        /// <param name="source">The source that logs.</param>
        /// <param name="payload">The payload to log.</param>
        /// <param name="eventType">Type of the event that causes the log.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        void Log(string source, object o, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null);

        /// <summary>
        ///     Builds a log entry from the given parameters and logs it if it passes the filter.
        /// </summary>
        /// <param name="source">The source that logs.</param>
        /// <param name="payload">The payload to log.</param>
        /// <param name="eventType">Type of the event that causes the log.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        void Log(string source, object[] payload, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null);

        /// <summary>
        ///     Builds a log entry from the given parameters and logs it if it passes the filter.
        /// </summary>
        /// <param name="eventType">Type of the event that causes the log.</param>
        /// <param name="source">The source that logs.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        /// <param name="title">Title of the log.</param>
        /// <param name="payload">The payload to log.</param>
        /// <param name="traceOptions">Tracing options that should be added to the log.</param>
        /// <param name="indentLevel">Level of indent at which the log should be logged.</param>
        /// <param name="indentSize">Size of the indents.</param>
        void Log(
            TraceEventType eventType = TraceEventType.Verbose,
            string source = null,
            [CallerMemberName] string tag = null,
            string title = null,
            IList<object> payload = null,
            TraceOptions traceOptions = LogEntry.DefaultOptions,
            ushort indentLevel = 0,
            ushort indentSize = 0);

        /// <summary>Releases all resources.</summary>
        /// <param name="isDisposing">
        ///     Indicates whether the <see cref="Dispose"/> method has been called before.
        /// </param>
        void Dispose(bool isDisposing);
    }
}