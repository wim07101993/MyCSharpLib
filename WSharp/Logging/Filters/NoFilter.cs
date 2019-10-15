namespace WSharp.Logging.Filters
{
    public class NoFilter : ILogFilter
    {
        public bool FilterLog(ILogEntry log) => true;
    }
}