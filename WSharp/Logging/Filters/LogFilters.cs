using System.Collections.Generic;
using System.Linq;

namespace WSharp.Logging.Filters
{
    /// <summary>Extension methods for log filters.</summary>
    public static class LogFilterExtensions
    {
        /// <summary>Combines multiple filters in an AND relation.</summary>
        /// <param name="filters">Filters to combine.</param>
        /// <returns>The combined log filter.</returns>
        public static ILogFilter CombineAnd(params ILogFilter[] filters) => new CombinedAndFilter { Filters = filters };

        /// <summary>Combines multiple filters in an AND relation.</summary>
        /// <param name="filters">Filters to combine.</param>
        /// <returns>The combined log filter.</returns>
        public static ILogFilter CombineAnd(IEnumerable<ILogFilter> filters) => CombineAnd(filters.ToArray());

        /// <summary>Combines multiple filters in an OR relation.</summary>
        /// <param name="filters">Filters to combine.</param>
        /// <returns>The combined log filter.</returns>
        public static ILogFilter CombineOr(params ILogFilter[] filters) => new CombinedOrFilter { Filters = filters };

        /// <summary>Combines multiple filters in an OR relation.</summary>
        /// <param name="filters">Filters to combine.</param>
        /// <returns>The combined log filter.</returns>
        public static ILogFilter CombineOr(IEnumerable<ILogFilter> filters) => CombineOr(filters.ToArray());

        #region EXTENSION METHODS

        /// <summary>
        /// Combines this filter with another one in an AND relation.
        /// </summary>
        /// <param name="filter">This filter.</param>
        /// <param name="filterToAdd">Filter to AND with this filter.</param>
        /// <returns>The combined filter.</returns>
        public static ILogFilter And(this ILogFilter filter, ILogFilter filterToAdd) => CombineAnd(filter, filterToAdd);

        /// <summary>
        /// Combines this filter with another one in an OR relation.
        /// </summary>
        /// <param name="filter">This filter.</param>
        /// <param name="filterToAdd">Filter to OR with this filter.</param>
        /// <returns>The combined filter.</returns>
        public static ILogFilter Or(this ILogFilter filter, ILogFilter filterToAdd) => CombineOr(filter, filterToAdd);

        #endregion EXTENSION METHODS
    }
}