using System.Collections.Generic;
using System.Diagnostics;

namespace MyCSharpLib.Services.Logging
{
    public class BufferLogEntry : IBufferLogEntry
    {
        public string Source { get; set; }
        public string Tag { get; set; }
        public TraceEventType EventType { get; set; } = TraceEventType.Verbose;
        public string Title { get; set; }

        public List<object> Payload { get; set; } = new List<object>();

        public TraceOptions TraceOptions { get; set; } = LogEntry.DefaultOptions;

        public ushort IndentLevel { get; set; }

        public ushort IndentSize { get; set; }
    }
}
