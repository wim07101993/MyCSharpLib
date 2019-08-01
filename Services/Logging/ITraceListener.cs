using System;
using System.Diagnostics;

namespace MyCSharpLib.Services.Logging
{
    public interface ITraceListener
    {
        void Close();
        void Flush();

        void Fail(string message);
        void Fail(string message, string detailMessage);

        void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data);
        void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data);

        void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id);
        void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message);
        void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args);

        void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId);

        void Write(string message);
        void Write(string message, string category);

        void WriteLine(string message);
        void WriteLine(string message, string category);

        void Write(object o);
        void Write(object o, string category);

        void WriteLine(object o);
        void WriteLine(object o, string category);
    }
}
