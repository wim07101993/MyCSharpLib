namespace MyCSharpLib.Services.Logging.Filters
{
    public class BlockFilter : ILogFilter
    {
        public bool FilterLog(ILogEntry log) => false;
    }
}
