namespace WSharp.Logging.Loggers
{
    public interface IFileLogger : ITextLogger
    {
        string LogDirectory { get; set; }
    }
}