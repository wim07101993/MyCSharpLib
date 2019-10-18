using System.Collections.Generic;
using System.Diagnostics;

namespace WSharp.Logging
{
    public interface IBufferLogEntry
    {
        /// <summary>The source that logs.</summary>
        string Source { get; set; }

        /// <summary>A tag to identify the log (by default the name of the caller).</summary>
        string Tag { get; set; }

        /// <summary>Type of the event that causes the log.</summary>
        TraceEventType EventType { get; set; }

        /// <summary>Title of the log.</summary>
        string Title { get; set; }

        /// <summary>The payload to log.</summary>
        List<object> Payload { get; set; }

        /// <summary>Tracing options that should be added to the log.</summary>
        TraceOptions TraceOptions { get; set; }

        /// <summary>Level of indent at which the log should be logged.</summary>
        ushort IndentLevel { get; set; }

        /// <summary>Size of the indents.</summary>
        ushort IndentSize { get; set; }
    }
}