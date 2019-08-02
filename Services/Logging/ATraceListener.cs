using MyCSharpLib.Services.Serialization.Extensions;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Logging
{
    public abstract class ATraceListener : TraceListener, ILogger
    {
        #region CONSTRUCTOR

        protected ATraceListener()
        {
            Filter = TraceFilters.NoFilter;
        }

        protected ATraceListener(TraceFilter filter)
        {
            Filter = filter;
        }

        #endregion CONSTRUCTOR


        #region PROPERTIES

        public TraceEventType DefaultTraceEventType { get; set; } = TraceEventType.Verbose;

        #endregion PROPERTIES


        #region METHODS

        public abstract Task WriteAsync(string message);

        public virtual async Task WriteLineAsync(string message) => await WriteAsync($"{message}\r\n");

        public virtual async Task WriteAsync(string message, TraceEventType eventType)
        {
            if (!ShouldTrace(eventType))
                return;

            await WriteAsync($"{eventType}: {message}");
        }

        public virtual async Task WriteLineAsync(string message, TraceEventType eventType) => await WriteAsync($"{message}\r\n", eventType);

        protected virtual bool ShouldTrace(TraceEventType eventType)
            => Filter == null || Filter.ShouldTrace(null, "", eventType, 0, null, null, null, null);


        #region overridden
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void

        public override void Write(string message)
        {
            if (!ShouldTrace(DefaultTraceEventType))
                return;

            WriteAsync(message);
        }

        public override void Write(string message, string category)
        {
            if (!ShouldTrace(DefaultTraceEventType))
                return;

            WriteAsync($"{category}: {message}");
        }

        public override void WriteLine(string message)
        {
            if (!ShouldTrace(DefaultTraceEventType))
                return;

            Write($"{message}\r\n");
        }

        public override void WriteLine(string message, string category)
        {
            if (!ShouldTrace(DefaultTraceEventType))
                return;

            Write($"{message}\r\n", category);
        }

        public override async void Write(object o)
        {
            if (!ShouldTrace(DefaultTraceEventType))
                return;

            if (o is LogEntry log)
                Write(log);
            else
            {
                var json = await o.SerializeJsonAsync();
                Write(json);
            }
        }

        public override async void Write(object o, string category)
        {
            if (!ShouldTrace(DefaultTraceEventType))
                return;

            var json = await o.SerializeJsonAsync();
            Write(json, category);
        }

        public override async void WriteLine(object o)
        {
            if (!ShouldTrace(DefaultTraceEventType))
                return;

            var json = await o.SerializeJsonAsync();
            WriteLine(json);
        }

        public override async void WriteLine(object o, string category)
        {
            if (!ShouldTrace(DefaultTraceEventType))
                return;

            var json = await o.SerializeJsonAsync();
            WriteLine(json, category);
        }

#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        #endregion overridden

        #endregion METHODS
    }
}
