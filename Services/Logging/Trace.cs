using MyCSharpLib.Services.Serialization.Extensions;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace MyCSharpLib.Services.Logging
{
    public static class Trace
    {
        public static void WriteLineIndented(string message)
        {
            System.Diagnostics.Trace.Indent();
            System.Diagnostics.Trace.WriteLine(message);
            System.Diagnostics.Trace.Unindent();
        }

        public static void WriteLines(params string[] args)
        {
            if (args == null || args.Length == 0)
            {
                System.Diagnostics.Trace.WriteLine("");
                return;
            }
            System.Diagnostics.Trace.WriteLine(args[0]);
            System.Diagnostics.Trace.Indent();

            if (args.Length == 2)
                System.Diagnostics.Trace.WriteLine(args[1]);
            else
                for (var i = 1; i < args.Length; i++)
                    System.Diagnostics.Trace.WriteLine(args[i]);
            System.Diagnostics.Trace.Unindent();
        }

        public static void WriteLines(params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                System.Diagnostics.Trace.WriteLine("");
                return;
            }
            System.Diagnostics.Trace.WriteLine(ToString(args[0]));
            System.Diagnostics.Trace.Indent();

            if (args.Length == 2)
                System.Diagnostics.Trace.WriteLine(ToString(args[1]));
            else
                for (var i = 1; i < args.Length; i++)
                    System.Diagnostics.Trace.WriteLine(ToString(args[i]));
            System.Diagnostics.Trace.Unindent();
        }

        private static string ToString(object obj)
        {
            switch (obj)
            {
                case string s:
                    return s;
                case byte[] bytes:
                {
                    var bytesBuilder = new StringBuilder("[ ");

                    foreach (var b in bytes)
                        bytesBuilder.Append(b).Append("");

                    bytesBuilder.Append("]");
                    return bytesBuilder.ToString();
                }
                default:
                {
                    try
                    {
                        return obj.SerializeJson();
                    }
                    catch (JsonException)
                    {
                        return obj.ToString();
                    }
                }   
            }
        }

        #region FORWARDERS

        public static bool UseGlobalLock
        {
            get => System.Diagnostics.Trace.UseGlobalLock;
            set => System.Diagnostics.Trace.UseGlobalLock = value;
        }

        public static int IndentSize
        {
            get => System.Diagnostics.Trace.IndentSize;
            set => System.Diagnostics.Trace.IndentSize = value;
        }

        public static int IndentLevel
        {
            get => System.Diagnostics.Trace.IndentLevel;
            set => System.Diagnostics.Trace.IndentLevel = value;
        }

        public static CorrelationManager CorrelationManager => System.Diagnostics.Trace.CorrelationManager;

        public static bool AutoFlush
        {
            get => System.Diagnostics.Trace.AutoFlush;
            set => System.Diagnostics.Trace.AutoFlush = value;
        }

        public static TraceListenerCollection Listeners => System.Diagnostics.Trace.Listeners;

        [Conditional("TRACE")]
        public static void Assert(bool condition) => System.Diagnostics.Trace.Assert(condition);
        [Conditional("TRACE")]
        public static void Assert(bool condition, string message) => System.Diagnostics.Trace.Assert(condition, message);
        [Conditional("TRACE")]
        public static void Assert(bool condition, string message, string detailMessage) => System.Diagnostics.Trace.Assert(condition, message, detailMessage);
        [Conditional("TRACE")]
        public static void Close() => System.Diagnostics.Trace.Close();
        [Conditional("TRACE")]
        public static void Fail(string message, string detailMessage) => System.Diagnostics.Trace.Fail(message, detailMessage);
        [Conditional("TRACE")]
        public static void Fail(string message) => System.Diagnostics.Trace.Fail(message);
        [Conditional("TRACE")]
        public static void Flush() => System.Diagnostics.Trace.Flush();
        [Conditional("TRACE")]
        public static void Indent() => System.Diagnostics.Trace.Indent();
        public static void Refresh() => System.Diagnostics.Trace.Refresh();
        [Conditional("TRACE")]
        public static void TraceError(string message) => System.Diagnostics.Trace.TraceError(message);
        [Conditional("TRACE")]
        public static void TraceError(string format, params object[] args) => System.Diagnostics.Trace.TraceError(format, args);
        [Conditional("TRACE")]
        public static void TraceInformation(string message) => System.Diagnostics.Trace.TraceInformation(message);
        [Conditional("TRACE")]
        public static void TraceInformation(string format, params object[] args) => System.Diagnostics.Trace.TraceInformation(format, args);
        [Conditional("TRACE")]
        public static void TraceWarning(string message) => System.Diagnostics.Trace.TraceWarning(message);
        [Conditional("TRACE")]
        public static void TraceWarning(string format, params object[] args) => System.Diagnostics.Trace.TraceWarning(format, args);
        [Conditional("TRACE")]
        public static void Unindent() => System.Diagnostics.Trace.Unindent();
        [Conditional("TRACE")]
        public static void Write(string message) => System.Diagnostics.Trace.Write(message);
        [Conditional("TRACE")]
        public static void Write(string message, string category) => System.Diagnostics.Trace.Write(message, category);
        [Conditional("TRACE")]
        public static void Write(object value, string category) => System.Diagnostics.Trace.Write(ToString(value), category);
        [Conditional("TRACE")]
        public static void Write(object value) => System.Diagnostics.Trace.Write(ToString(value));
        [Conditional("TRACE")]
        public static void WriteIf(bool condition, string message, string category) => System.Diagnostics.Trace.WriteIf(condition, message, category);
        [Conditional("TRACE")]
        public static void WriteIf(bool condition, object value) => System.Diagnostics.Trace.WriteIf(condition, ToString(value));
        [Conditional("TRACE")]
        public static void WriteIf(bool condition, object value, string category) => System.Diagnostics.Trace.WriteIf(condition, ToString(value), category);
        [Conditional("TRACE")]
        public static void WriteIf(bool condition, string message) => System.Diagnostics.Trace.WriteIf(condition, message);
        [Conditional("TRACE")]
        public static void WriteLine(object value) => System.Diagnostics.Trace.WriteLine(ToString(value));
        [Conditional("TRACE")]
        public static void WriteLine(object value, string category) => System.Diagnostics.Trace.WriteLine(ToString(value), category);
        [Conditional("TRACE")]
        public static void WriteLine(string message) => System.Diagnostics.Trace.WriteLine(message);
        [Conditional("TRACE")]
        public static void WriteLine(string message, string category) => System.Diagnostics.Trace.WriteLine(message, category);
        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, object value) => System.Diagnostics.Trace.WriteLineIf(condition, ToString(value));
        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, object value, string category) => System.Diagnostics.Trace.WriteLineIf(condition, ToString(value), category);
        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, string message) => System.Diagnostics.Trace.WriteLineIf(condition, message);
        [Conditional("TRACE")]
        public static void WriteLineIf(bool condition, string message, string category) => System.Diagnostics.Trace.WriteLineIf(condition, message, category);

        #endregion FORWARDERS
    }
}
