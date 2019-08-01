using System.Diagnostics;

namespace MyCSharpLib.Services.Logging
{
    public interface ITraceSourceFactory
    {
        TraceSource Resolve(string name, SourceLevels defaultLevel = SourceLevels.All);

        ITraceSourceFactory RegisterListener<T>() where T : ITraceListener;
        ITraceSourceFactory RegisterListener<T>(T listener) where T : ITraceListener;
        ITraceSourceFactory RegisterListener(TraceListener listener);
    }
}
