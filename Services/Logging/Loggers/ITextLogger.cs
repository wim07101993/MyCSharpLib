using System.IO;

namespace MyCSharpLib.Services.Logging.Loggers
{
    public interface ITextLogger : ILogger
    {
        TextWriter Writer { get; }
    }
}
