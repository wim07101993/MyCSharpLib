using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MyCSharpLib.Services.Logging.Loggers
{
    public interface ILogger : IDisposable
    {
        object GlobalLock { get; }

        bool ShouldLog { get; set; }

        ILogFilter Filter { get; set; }

        IBufferLogEntry Buffer { get; }

        void Log(IBufferLogEntry logEntry);
        void Log(ILogEntry logEntry);
        void LogBuffer();
        
        void Log(string source, object o, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null);
        void Log(string source, object[] payload, TraceEventType eventType = TraceEventType.Verbose, [CallerMemberName] string tag = null);


        void Log(
            TraceEventType eventType = TraceEventType.Verbose,
            string source = null,
            [CallerMemberName] string tag = null,
            string title = null,
            IList<object> payload = null,
            TraceOptions traceOptions = LogEntry.DefaultOptions,
            ushort indentLevel = 0,
            ushort indentSize = 0);

        void Dispose(bool isDisposing);
    }
}
