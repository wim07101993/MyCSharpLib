using System.Collections.Generic;
using System.Linq;

namespace MyCSharpLib.Services.Logging.Filters
{
    public static class LogFilterExtensions
    {
        public static ILogFilter CombineAnd(params ILogFilter[] filters) => new CombinedAndFilter { Filters = filters };
        public static ILogFilter CombineAnd(IEnumerable<ILogFilter> filters) => CombineAnd(filters.ToArray());

        public static ILogFilter CombineOr(params ILogFilter[] filters) => new CombinedOrFilter { Filters = filters };
        public static ILogFilter CombineOr(IEnumerable<ILogFilter> filters) => CombineOr(filters.ToArray());

        
        #region EXTENSION METHODS

        public static ILogFilter And(this ILogFilter filter, ILogFilter filterToAdd) => CombineAnd(filter, filterToAdd);

        public static ILogFilter Or(this ILogFilter filter, ILogFilter filterToAdd) => CombineOr(filter, filterToAdd);

        #endregion EXTENSION METHODS
    }
}
