using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WSharp.Logging
{
    public interface ILogEntry
    {
        /// <summary>Id of the log.</summary>
        Guid Id { get; }

        /// <summary>The source that logs.</summary>
        string Source { get; }

        /// <summary>A tag to identify the log (by default the name of the caller).</summary>
        string Tag { get; }

        /// <summary>Type of the event that causes the log.</summary>
        TraceEventType EventType { get; }

        /// <summary>Title of the log.</summary>
        string Title { get; }

        /// <summary>The payload to log.</summary>
        IReadOnlyList<object> Payload { get; }

        /// <summary>Provides trace event data specific to a thread and a process.</summary>
        TraceEventCache EventCache { get; }

        /// <summary>Tracing options that should be added to the log.</summary>
        TraceOptions TraceOptions { get; }

        /// <summary>Level of indent at which the log should be logged.</summary>
        ushort IndentLevel { get; }

        /// <summary>Size of the indents.</summary>
        ushort IndentSize { get; }

        /// <summary>Header of the log entry.</summary>
        string Header { get; }

        /// <summary>Body of the log entry.</summary>
        string Body { get; }

        /// <summary>Footer of the log entry.</summary>
        string Footer { get; }
    }
}