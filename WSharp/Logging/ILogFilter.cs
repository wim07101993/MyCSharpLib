namespace WSharp.Logging
{
    public interface ILogFilter
    {
        bool FilterLog(ILogEntry log);
    }
}