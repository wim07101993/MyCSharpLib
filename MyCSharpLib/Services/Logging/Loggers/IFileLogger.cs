namespace MyCSharpLib.Services.Logging.Loggers
{
    public interface IFileLogger : ITextLogger
    {
        string LogDirectory { get; set; }
    }
}
