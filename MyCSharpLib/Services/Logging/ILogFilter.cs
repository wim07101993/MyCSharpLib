namespace MyCSharpLib.Services.Logging
{
    public interface ILogFilter
    {
        bool FilterLog(ILogEntry log);
    }
}
