using System.Collections.Generic;
using System.Diagnostics;

namespace WSharp.Logging
{
    /// <summary>A log entry to keep in memory until it can be logged.</summary>
    public class BufferLogEntry : IBufferLogEntry
    {
        /// <summary>The source that logs.</summary>
        public string Source { get; set; }

        /// <summary>A tag to identify the log (by default the name of the caller).</summary>
        public string Tag { get; set; }

        /// <summary>Type of the event that causes the log.</summary>
        public TraceEventType EventType { get; set; } = TraceEventType.Verbose;

        /// <summary>Title of the log.</summary>
        public string Title { get; set; }

        /// <summary>The payload to log.</summary>
        public List<object> Payload { get; set; } = new List<object>();

        /// <summary>Tracing options that should be added to the log.</summary>
        public TraceOptions TraceOptions { get; set; } = LogEntry.DefaultOptions;

        /// <summary>Level of indent at which the log should be logged.</summary>
        public ushort IndentLevel { get; set; }

        /// <summary>Size of the indents.</summary>
        public ushort IndentSize { get; set; }
    }
}
