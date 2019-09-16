using System.Linq;

namespace WSharp.Services.Logging.Filters
{
    public class CombinedOrFilter : ILogFilter
    {

        public ILogFilter[] Filters { get; set; }

        public bool FilterLog(ILogEntry log) => Filters.Any(x => x.FilterLog(log));
    }
}
