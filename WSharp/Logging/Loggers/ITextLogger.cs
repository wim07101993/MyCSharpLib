using System.IO;

namespace WSharp.Logging.Loggers
{
    public interface ITextLogger : ILogger
    {
        TextWriter Writer { get; }
    }
}