using System.Linq;

namespace MyCSharpLib.Services.Logging.Filters
{
    public class CombinedAndFilter : ILogFilter
    {
        public ILogFilter[] Filters { get; set; }

        public bool FilterLog(ILogEntry log) => Filters.All(x => x.FilterLog(log));
    }
}
