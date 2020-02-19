namespace WSharp.Logging.Loggers
{
    /// <summary>Logs to a file.</summary>
    public interface IFileLogger : ITextLogger
    {
        /// <summary>Directory to log to.</summary>
        string LogDirectory { get; set; }
    }
}
