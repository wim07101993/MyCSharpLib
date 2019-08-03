using System.Collections.Generic;
using System.Diagnostics;

namespace MyCSharpLib.Services.Logging
{
    public interface ILogEntry
    {
        string Source { get; }
        string Tag { get; }
        TraceEventType EventType { get; }
        string Title { get; }

        IReadOnlyList<object> Payload { get; }

        TraceEventCache EventCache { get; }
        TraceOptions TraceOptions { get; }

        ushort IndentLevel { get; }
        ushort IndentSize { get; }

        string Header { get; }
        string Body { get; }
        string Footer { get; }
    }
}
