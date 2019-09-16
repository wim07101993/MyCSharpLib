using WSharp.Services.Logging.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WSharp.Services.Logging.Loggers
{
    public abstract class ALogger : ILogger, ILogFilter
    {
        #region FIELDS

        internal static readonly object globalLock = new object();

        private bool _isDisposing;
        private IBufferLogEntry _buffer;

        #endregion FIELDS


        #region CONSTRUCTOR

        protected ALogger()
        {
            Filter = new NoFilter();
        }

        protected ALogger(ILogFilter filter)
        {
            Filter = filter;
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        public object GlobalLock => globalLock;

        public bool ShouldLog { get; set; } = true;

        public ILogFilter Filter { get; set; }

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

        public virtual void Dispose(bool isDisposing)
        {
        }

        public abstract void InternalLog(ILogEntry logEntry);

        public virtual bool FilterLog(ILogEntry logEntry)
            => ShouldLog && (Filter == null || Filter.FilterLog(logEntry));

        public virtual void Log(ILogEntry logEntry)
        {
            if (!FilterLog(logEntry))
                return;

            lock(GlobalLock)
            {
                InternalLog(logEntry);
            }
        }

        public virtual void Log(IBufferLogEntry logEntry) => Log(new LogEntry(logEntry));

        public void Log(string source, object payload, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null)
            => Log(new LogEntry(source, tag, payload: new[] { payload }, eventType: eventType));

        public void Log(string source, object[] payload, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null)
            => Log(new LogEntry(source, tag, payload: payload, eventType: eventType));

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
               title, new ReadOnlyCollection<object>(payload),
               traceOptions,
               indentLevel,
               indentSize));

        public virtual void LogBuffer()
        {
            Log(Buffer);
            _buffer = null;
        }

        public void LogInternalException(Exception e, [CallerMemberName] string tag = null)
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
