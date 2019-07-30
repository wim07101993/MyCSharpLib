using System.Diagnostics;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Logging
{
    public interface ILogger
    {
        TraceEventType DefaultTraceEventType { get; set; }

        Task WriteAsync(string message);
        Task WriteLineAsync(string message);

        Task WriteAsync(string message, TraceEventType eventType);
        Task WriteLineAsync(string message, TraceEventType eventType);
    }
}
