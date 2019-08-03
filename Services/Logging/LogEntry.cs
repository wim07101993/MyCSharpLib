using MyCSharpLib.Services.Serialization.Extensions;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyCSharpLib.Services.Logging
{
    public class LogEntry : ILogEntry
    {
        #region FIELDS

        public const TraceOptions AllTraceOptions =
            TraceOptions.LogicalOperationStack |
            TraceOptions.DateTime |
            TraceOptions.Timestamp |
            TraceOptions.ProcessId |
            TraceOptions.ThreadId |
            TraceOptions.Callstack;

        public const TraceOptions DefaultOptions = TraceOptions.DateTime;
        #endregion FIELDS


        #region CONSTRUCTOR

        public LogEntry(
            string source = null,
            [CallerMemberName] string tag = null, 
            TraceEventType eventType = TraceEventType.Verbose,  
            string title = null, 
            IReadOnlyList<object> payload = null, 
            TraceOptions traceOptions = DefaultOptions, 
            ushort indentLevel = 0, 
            ushort indentSize = 0)
        {
            if (!string.IsNullOrWhiteSpace(source))
                Source = source;
            if (!string.IsNullOrWhiteSpace(tag))
                Tag = tag;

            EventType = eventType;

            if (!string.IsNullOrWhiteSpace(title))
                Title = title;

            Payload = payload;

            EventCache = new TraceEventCache();
            TraceOptions = traceOptions;            

            IndentLevel = indentLevel;
            IndentSize = indentSize;
        }

        public LogEntry(IBufferLogEntry buffer)
            : this(buffer.Source,
                  buffer.Tag,
                  buffer.EventType,
                  buffer.Title,
                  buffer.Payload,
                  buffer.TraceOptions,
                  buffer.IndentLevel,
                  buffer.IndentSize)
        { }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        public string Source { get; }
        public string Tag { get; }
        public TraceEventType EventType { get; }
        public string Title { get; }

        public IReadOnlyList<object> Payload { get; }

        public TraceEventCache EventCache { get; }
        public TraceOptions TraceOptions { get; }

        public ushort IndentLevel { get; }
        public ushort IndentSize { get; }

        public virtual string Header
        {
            get
            {
                var builder = new StringBuilder().Append(' ', IndentSize * IndentLevel);

                if (EventCache != null && IsEnabled(TraceOptions.DateTime))
                    builder
                        .Append(EventCache.DateTime.ToString(CultureInfo.InvariantCulture))
                        .Append("|");

                if (!string.IsNullOrWhiteSpace(Source))
                    builder.Append(Source).Append(":");

                if (!string.IsNullOrWhiteSpace(Tag))
                    builder.Append(Tag).Append(":");

                if (EventCache != null && IsEnabled(TraceOptions.DateTime) &&
                    !string.IsNullOrWhiteSpace(Source) && string.IsNullOrWhiteSpace(Tag))
                    builder.Append(EventType).Append(" -> ");

                if (!string.IsNullOrWhiteSpace(Title))
                    builder.AppendLine(Title);

                return builder.ToString();
            }
        }

        public virtual string Body
        {
            get
            {
                if (Payload == null || Payload.Count == 0)
                    return null;

                var builder = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(Title))
                    builder.Append(' ', IndentSize * (IndentLevel + 2));

                builder.AppendLine(PayloadToString(Payload[0])).ToString();
                if (Payload.Count == 1)
                    return builder.ToString();

                var slice = Payload.Skip(1).ToList();
                var strPayload = PayloadToString(slice, IndentSize, (ushort)(IndentLevel + 1));

                return builder.AppendLine(strPayload).ToString();
            }
        }

        public string Footer
        {
            get
            {
                if (EventCache == null)
                    return null;

                var builder = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(Title))
                    builder.Append(' ', IndentSize * (IndentLevel + 1));

                var hasFooter = false;
                if (IsEnabled(TraceOptions.ProcessId))
                {
                    hasFooter = true;
                    builder.Append("ProcessId=").Append(EventCache.ProcessId);
                }

                if (IsEnabled(TraceOptions.LogicalOperationStack))
                {
                    hasFooter = true;
                    builder.Append("LogicalOperationStack=");
                    Stack operationStack = EventCache.LogicalOperationStack;
                    builder.Append(OperationStackToString(operationStack));
                }

                if (IsEnabled(TraceOptions.ThreadId))
                {
                    hasFooter = true;
                    builder.Append("ThreadId=").Append(EventCache.ThreadId);
                }

                if (IsEnabled(TraceOptions.Timestamp))
                {
                    hasFooter = true;
                    builder.Append("Timestamp=").Append(EventCache.Timestamp);
                }

                if (IsEnabled(TraceOptions.Callstack))
                {
                    hasFooter = true;
                    builder.Append("Callstack=").Append(EventCache.Callstack);
                }

                return hasFooter
                    ? builder.AppendLine().ToString() 
                    : null;
            }
        }

        #endregion PROPERTIES


        #region METHODS

        public virtual string PayloadToString(IList payLoad, ushort indentSize, ushort indentLevel)
        {
            if (payLoad == null || payLoad.Count == 0)
                return null;

            var builder = new StringBuilder();
            foreach (var p in payLoad)
                builder.Append(' ', indentSize * indentLevel)
                    .AppendLine(PayloadToString(payLoad));

            return builder.ToString();
        }

        public virtual string OperationStackToString(Stack stack)
        {
            if (stack == null || stack.Count == 0)
                return null;

            var list = stack.Cast<object>().ToList();

            var builder = new StringBuilder(list[0].ToString());
            foreach (var item in list.Skip(1))
                builder.Append(", ").Append(item);

            return builder.AppendLine().ToString();
        }

        public virtual string PayloadToString(object payload)
        {
            switch (payload)
            {
                case string s:
                    return s;
                default:
                    return payload.SerializeJson(Formatting.Indented);
            }
        }

        public override string ToString() => $"{Header}{Body}{Footer}";

        protected virtual bool IsEnabled(TraceOptions opts) => (opts & TraceOptions) != 0;

        #endregion METHODS
    }
}
