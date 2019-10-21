using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WSharp.Logging.Filters;

namespace WSharp.Logging.Loggers
{
    /// <summary>
    ///     Abstract class that implements the <see cref="ILogger"/> and <see cref="ILogFilter"/> interface.
    /// </summary>
    public abstract class ALogger : ALogFilter, ILogger
    {
        #region FIELDS

        internal static readonly object globalLock = new object();

        private bool _isDisposing;
        private IBufferLogEntry _buffer;

        #endregion FIELDS

        #region CONSTRUCTOR

        /// <summary>Initiates the object with a <see cref="NoFilter"/> as <see cref="Filter"/>.</summary>
        protected ALogger()
        {
            Filter = new NoFilter();
        }

        /// <summary>Initiates the object with a filter.</summary>
        /// <param name="filter">Filter to filter the log entries to log.</param>
        protected ALogger(ILogFilter filter)
        {
            Filter = filter;
        }

        #endregion CONSTRUCTOR

        #region PROPERTIES

        /// <summary>A lock to synchronize tasks/threads.</summary>
        public object GlobalLock => globalLock;

        /// <summary>If <see langword="false"/>: no logs are logged.</summary>
        public bool ShouldLog { get; set; } = true;

        /// <summary>Filter that filters what logs should be logged.</summary>
        public ILogFilter Filter { get; set; }

        /// <summary>
        ///     A buffer to write to and in the end log with the <see cref="Log(IBufferLogEntry)"/> or <see cref="LogBuffer()"/> method.
        /// </summary>
        public IBufferLogEntry Buffer
        {
            get
            {
                if (_buffer == null)
                    _buffer = new BufferLogEntry();
                return _buffer;
            }
        }

        #endregion PROPERTIES

        #region METHODS

        /// <summary>Releases all resources.</summary>
        public void Dispose()
        {
            lock (GlobalLock)
            {
                if (_isDisposing)
                    Dispose(true);
                else
                {
                    _isDisposing = true;
                    Dispose(false);
                }
            }
        }

        /// <summary>Releases all resources.</summary>
        /// <param name="isDisposing">
        ///     Indicates whether the <see cref="Dispose"/> method has been called before.
        /// </param>
        public virtual void Dispose(bool isDisposing)
        {
        }

        /// <summary>Method that actually logs the log entry.</summary>
        /// <param name="logEntry">Entry to log.</param>
        protected abstract void InternalLog(ILogEntry logEntry);

        /// <summary>Determines whether a log entry should be logged.</summary>
        /// <param name="logEntry">Log entry to check.</param>
        /// <returns>Whether the log entry should be logged.</returns>
        public override bool CanLog(ILogEntry logEntry)
            => ShouldLog && (Filter == null || Filter.CanLog(logEntry));

        /// <summary>Logs the given entry if it passes the logfilter.</summary>
        /// <param name="logEntry">Log entry to log.``</param>
        public virtual void Log(ILogEntry logEntry)
        {
            if (!CanLog(logEntry))
                return;

            lock (GlobalLock)
            {
                InternalLog(logEntry);
            }
        }

        /// <summary>Logs the given entry if it passes the logfilter.</summary>
        /// <param name="logEntry">Log entry to log.</param>
        public virtual void Log(IBufferLogEntry logEntry) => Log(new LogEntry(logEntry));

        /// <summary>
        ///     Builds a log entry from the given parameters and logs it if it passes the filter.
        /// </summary>
        /// <param name="source">The source that logs.</param>
        /// <param name="payload">The payload to log.</param>
        /// <param name="eventType">Type of the event that causes the log.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        public void Log(string source, object payload, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null)
            => Log(source, payload == null ? null : new[] { payload }, eventType, tag);

        /// <summary>
        ///     Builds a log entry from the given parameters and logs it if it passes the filter.
        /// </summary>
        /// <param name="source">The source that logs.</param>
        /// <param name="payload">The payload to log.</param>
        /// <param name="eventType">Type of the event that causes the log.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        public void Log(string source, object[] payload, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null)
            => Log(new LogEntry(source, tag, payload: payload, eventType: eventType));

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
        public virtual void Log(
            TraceEventType eventType = TraceEventType.Verbose,
            string source = null,
            [CallerMemberName] string tag = null,
            string title = null,
            IList<object> payload = null,
            TraceOptions traceOptions = TraceOptions.DateTime,
            ushort indentLevel = 0,
            ushort indentSize = 0)
            => Log(new LogEntry(
               source,
               tag,
               eventType,
               title,
               payload == null ? null : new ReadOnlyCollection<object>(payload),
               traceOptions,
               indentLevel,
               indentSize));

        /// <summary>Logs the <see cref="Buffer"/>.</summary>
        public virtual void LogBuffer()
        {
            Log(Buffer);
            _buffer = null;
        }

        /// <summary>
        /// Logs an internal exception. Sets <see cref="ShouldLog"/> to false before logging the exception.
        /// </summary>
        /// <param name="e">Exception to log.</param>
        /// <param name="tag">A tag to identify the log (by default the name of the caller).</param>
        protected void LogInternalException(Exception e, [CallerMemberName] string tag = null)
        {
            ShouldLog = false;
            Log(new LogEntry(
                           e.Source,
                           tag,
                           TraceEventType.Error,
                           "An error happened while logging",
                           new[] { e.Message },
                           LogEntry.DefaultOptions));
            ShouldLog = true;
        }

        #endregion METHODS
    }
}