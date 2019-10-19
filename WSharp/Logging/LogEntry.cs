using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using WSharp.Extensions;

namespace WSharp.Logging
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
            ushort indentSize = 4)
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

        /// <summary>Id of the log.</summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>The source that logs.</summary>
        public string Source { get; }

        /// <summary>A tag to identify the log (by default the name of the caller).</summary>
        public string Tag { get; }

        /// <summary>Type of the event that causes the log.</summary>
        public TraceEventType EventType { get; }

        /// <summary>Title of the log.</summary>
        public string Title { get; }

        /// <summary>The payload to log.</summary>
        public IReadOnlyList<object> Payload { get; }

        /// <summary>Provides trace event data specific to a thread and a process.</summary>
        public TraceEventCache EventCache { get; }

        /// <summary>Tracing options that should be added to the log.</summary>
        public TraceOptions TraceOptions { get; }

        /// <summary>Level of indent at which the log should be logged.</summary>
        public ushort IndentLevel { get; }

        /// <summary>Size of the indents.</summary>
        public ushort IndentSize { get; }

        /// <summary>Header of the log entry.</summary>
        public virtual string Header
        {
            get
            {
                var builder = new StringBuilder().Append(' ', IndentSize * IndentLevel);

                var eventType = EventType.ToString();

                builder.Append("[");
                builder.Append(EventType);
                builder.Append("]");
                for (var i = eventType.Length; i < 11; i++)
                    builder.Append(' ');

                if (EventCache != null && IsEnabled(TraceOptions.DateTime))
                    builder
                        .Append(EventCache.DateTime.ToString(CultureInfo.InvariantCulture))
                        .Append("|");

                if (!string.IsNullOrWhiteSpace(Source))
                    builder.Append(Source).Append(":");

                if (!string.IsNullOrWhiteSpace(Tag))
                    builder.Append(Tag);

                if (!string.IsNullOrWhiteSpace(Title) ||
                    (Payload != null && Payload.Count > 0))
                    builder.Append(" -> ");
                else
                    builder.AppendLine();

                if (!string.IsNullOrWhiteSpace(Title))
                    builder.AppendLine(Title);

                return builder.ToString();
            }
        }

        /// <summary>Body of the log entry.</summary>
        public virtual string Body
        {
            get
            {
                if (Payload == null || Payload.Count == 0)
                    return null;

                var builder = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(Title))
                    builder.Append(' ', IndentSize * (IndentLevel + 2));

                Indent(ref builder, Payload[0], IndentSize, (ushort)(IndentLevel + 2), false);
                if (Payload.Count == 1)
                    return builder.ToString();

                var slice = Payload.Skip(1).ToList();
                var strPayload = PayloadToString(slice, IndentSize, (ushort)(IndentLevel + 2));

                return builder.Append(strPayload).ToString();
            }
        }

        /// <summary>Footer of the log entry.</summary>
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
                    builder
                        .Append("ProcessId=")
                        .Append(EventCache.ProcessId)
                        .Append("|");
                }

                if (IsEnabled(TraceOptions.LogicalOperationStack))
                {
                    hasFooter = true;
                    Stack operationStack = EventCache.LogicalOperationStack;
                    builder
                        .Append("LogicalOperationStack=")
                        .Append(OperationStackToString(operationStack))
                        .Append("|");
                }

                if (IsEnabled(TraceOptions.ThreadId))
                {
                    hasFooter = true;
                    builder
                        .Append("ThreadId=")
                        .Append(EventCache.ThreadId)
                        .Append("|");
                }

                if (IsEnabled(TraceOptions.Timestamp))
                {
                    hasFooter = true;
                    builder
                        .Append("Timestamp=")
                        .Append(EventCache.Timestamp)
                        .Append("|");
                }

                if (IsEnabled(TraceOptions.Callstack))
                {
                    hasFooter = true;
                    builder
                        .Append("Callstack=")
                        .Append(EventCache.Callstack)
                        .Append("|");
                }

                return hasFooter
                    ? builder.AppendLine().ToString()
                    : null;
            }
        }

        #endregion PROPERTIES

        #region METHODS

        /// <summary>Converts the payload to a string.</summary>
        /// <param name="payLoad">Payload to convert.</param>
        /// <param name="indentSize">Size of the indents.</param>
        /// <param name="indentLevel">Level of indent at which the log should be logged.</param>
        /// <returns>The string representation of the log payload.</returns>
        protected virtual string PayloadToString(IList payLoad, ushort indentSize, ushort indentLevel)
        {
            if (payLoad == null || payLoad.Count == 0)
                return null;

            var builder = new StringBuilder();
            foreach (var p in payLoad)
                Indent(ref builder, p, indentSize, indentLevel);

            return builder.ToString();
        }

        /// <summary>Converts a <see cref="Stack"/> to a string.</summary>
        /// <param name="stack">Stack to convert.</param>
        /// <returns>The string representation of the <see cref="Stack"/>.</returns>
        protected virtual string OperationStackToString(Stack stack)
        {
            if (stack == null || stack.Count == 0)
                return null;

            var list = stack.Cast<object>().ToList();

            var builder = new StringBuilder(list[0].ToString());
            foreach (var item in list.Skip(1))
                builder.Append(", ").Append(item);

            return builder.AppendLine().ToString();
        }

        /// <summary>Converts a payload object to a string.</summary>
        /// <param name="payLoad">Payload to convert.</param>
        /// <returns>The string representation of the log payload.</returns>
        protected virtual string PayloadToString(object payload)
        {
            switch (payload)
            {
                case null:
                    return null;

                case string s:
                    return s;

                default:
                    return payload.SerializeJson();
            }
        }

        /// <summary>
        ///     Adds a part of the payload to a <see cref="StringBuilder"/> as a string representation.
        /// </summary>
        /// <param name="builder">Builder to add the string representation to.</param>
        /// <param name="o">Object to add.</param>
        /// <param name="indentSize">Size of the indents.</param>
        /// <param name="indentLevel">Level of indent at which the log should be logged.</param>
        /// <param name="indentFirstLine">Indicates whether the first line should also be indented.</param>
        private void Indent(ref StringBuilder builder, object o, ushort indentSize, ushort indentLevel, bool indentFirstLine = true)
        {
            var strPayload = PayloadToString(o);
            IEnumerable<string> split = strPayload.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Count() == 0)
                return;

            if (!indentFirstLine)
            {
                builder.AppendLine(split.First());
                split = split.Skip(1);
            }

            foreach (var s in split)
                builder.Append(' ', indentSize * indentLevel)
                    .AppendLine(s);
        }

        /// <summary>
        /// Converts the log entry to string ({Header}{Body}{Footer}).
        /// </summary>
        /// <returns>{Header}{Body}{Footer}</returns>
        public override string ToString() => $"{Header}{Body}{Footer}";

        /// <summary>
        /// Checks whether the given options are enabled in the <see cref="TraceOptions"/>.
        /// </summary>
        /// <param name="opts">Options to check whether they are enabled.</param>
        /// <returns>Whether the given options are enabled in the <see cref="TraceOptions"/>.</returns>
        protected virtual bool IsEnabled(TraceOptions opts) => (opts & TraceOptions) != 0;

        #endregion METHODS
    }
}