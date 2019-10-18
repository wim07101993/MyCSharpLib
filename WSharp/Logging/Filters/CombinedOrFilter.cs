using System.Linq;

namespace WSharp.Logging.Filters
{
    /// <summary>Combines multiple filters in an OR relation.</summary>
    public class CombinedOrFilter : ILogFilter
    {
        /// <summary>Filters to use in the OR relation.</summary>
        public ILogFilter[] Filters { get; set; }

        /// <summary>Check whether a log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        public bool FilterLog(ILogEntry log) => Filters.Any(x => x.FilterLog(log));
    }
}