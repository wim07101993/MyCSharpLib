using System.Collections.Generic;
using System.Diagnostics;

namespace MyCSharpLib.Services.Logging
{
    public interface IBufferLogEntry
    {
        string Source { get; set; }
        string Tag { get; set; }
        TraceEventType EventType { get; set; }
        string Title { get; set; }

        List<object> Payload { get; set; }

        TraceOptions TraceOptions { get; set; }

        ushort IndentLevel { get; set; }
        ushort IndentSize { get; set; }
    }
}
