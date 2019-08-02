using MyCSharpLib.Extensions;
using System;
using System.Diagnostics;
using System.Text;

namespace MyCSharpLib.Services.Logging
{
    public class LogEntry
    {
        public LogEntry(TraceEventType eventType, string tag, params string[] message)
        {
            EventType = eventType;
            Tag = tag;
            Message = message;
        }


        public DateTime TimeStamp { get; } = DateTime.Now;

        public TraceEventType EventType { get; }

        public string[] Message { get; }
        public string Tag { get; }


        public string LinesToString()
        {
            if (Message == null || Message.Length == 0)
                return null;

            if (Message.Length == 1)
                return Message[1];

            var builder = new StringBuilder(Message[1]);
            foreach (var m in Message.Slice(1))
            {
                builder.Append(' ', Trace.IndentSize * Trace.IndentLevel);
                builder.AppendLine(m);
            }

            return builder.ToString();
        }

        public override string ToString()
            => $"{TimeStamp} - {Tag} -> {EventType}: {Message}";
    }
}
