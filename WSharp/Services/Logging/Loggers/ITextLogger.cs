using System.IO;

namespace WSharp.Services.Logging.Loggers
{
    public interface ITextLogger : ILogger
    {
        TextWriter Writer { get; }
    }
}
