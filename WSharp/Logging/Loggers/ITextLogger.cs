using System.IO;

namespace WSharp.Logging.Loggers
{
    /// <summary>Logger that logs to a <see cref="TextWriter"/>.</summary>
    public interface ITextLogger : ILogger
    {
        /// <summary>Text writer to log to.</summary>
        TextWriter Writer { get; }
    }
}
