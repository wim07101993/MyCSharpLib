using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WSharp.Logging.Filters
{
    /// <summary>
    ///     Class that abstracts the AND, OR and NOT methods of the <see cref="ILogFilter"/> interface.
    /// </summary>
    public abstract class ALogFilter : ILogFilter
    {
        /// <summary>Indicates whether the given log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns></returns>
        public abstract bool CanLog(ILogEntry log);

        #region and

        /// <summary>Combines this filter with another one in an AND relation.</summary>
        /// <param name="filter">This filter.</param>
        /// <param name="filterToAdd">Filter to AND with these filters.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter And(IEnumerable<ILogFilter> filterToAdd) => new AndFilter(this, new AndFilter(filterToAdd));

        /// <summary>Combines this filter with another one in an AND relation.</summary>
        /// <param name="filter">This filter.</param>
        /// <param name="filtersToAdd">Filter to AND with these filters.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter And(params ILogFilter[] filtersToAdd) => And(filtersToAdd as IEnumerable<ILogFilter>);

        /// <summary>
        ///     Combines this filter with other event types in an AND relation. This filter should
        ///     accept the log and all the given event types should be present.
        /// </summary>
        /// <param name="eventTypes">Filters to and.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter And(IEnumerable<TraceEventType> eventTypes) => new AndFilter(this, new AndFilter(eventTypes));

        /// <summary>
        ///     Combines this filter with other event types in an AND relation. This filter should
        ///     accept the log and all the given event types should be present.
        /// </summary>
        /// <param name="eventTypes">Filters to and.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter And(params TraceEventType[] eventTypes) => And(eventTypes as IEnumerable<TraceEventType>);

        /// <summary>
        ///     Combines this filter with other funcs in an AND relation. This filter should accept
        ///     the log if all the funcs agree.
        /// </summary>
        /// <param name="funcs">Funcs to and.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter And(IEnumerable<Func<ILogEntry, bool>> funcs) => new AndFilter(this, new AndFilter(funcs));

        /// <summary>
        ///     Combines this filter with other funcs in an AND relation. This filter should accept
        ///     the log if all the funcs agree.
        /// </summary>
        /// <param name="funcs">Funcs to and.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter And(params Func<ILogEntry, bool>[] funcs) => And(funcs as IEnumerable<Func<ILogEntry, bool>>);

        #endregion and

        #region or

        /// <summary>Combines this filter with another one in an OR relation.</summary>
        /// <param name="filter">This filter.</param>
        /// <param name="filtersToAdd">Filter to OR with these filters.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter Or(IEnumerable<ILogFilter> filtersToAdd) => new OrFilter(this, new OrFilter(filtersToAdd));

        /// <summary>Combines this filter with another one in an OR relation.</summary>
        /// <param name="filter">This filter.</param>
        /// <param name="filtersToAdd">Filter to OR with these filters.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter Or(params ILogFilter[] filtersToAdd) => Or(filtersToAdd as IEnumerable<ILogFilter>);

        /// <summary>
        ///     Combines this filter with other event types in an OR relation. This filter should
        ///     accept the log or any of the given event types should be present.
        /// </summary>
        /// <param name="eventTypes">Event types to or.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter Or(IEnumerable<TraceEventType> eventTypes) => new OrFilter(this, new OrFilter(eventTypes));

        /// <summary>
        ///     Combines this filter with other event types in an OR relation. This filter should
        ///     accept the log or any of the given event types should be present.
        /// </summary>
        /// <param name="eventTypes">Event types to or.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter Or(params TraceEventType[] eventTypes) => Or(eventTypes as IEnumerable<TraceEventType>);

        /// <summary>
        ///     Combines this filter with other funcs in an OR relation. This filter should accept
        ///     the log if any the funcs agree.
        /// </summary>
        /// <param name="funcs">Funcs to and.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter Or(IEnumerable<Func<ILogEntry, bool>> funcs) => new OrFilter(this, new OrFilter(funcs));

        /// <summary>
        ///     Combines this filter with other funcs in an OR relation. This filter should accept
        ///     the log if any the funcs agree.
        /// </summary>
        /// <param name="funcs">Funcs to and.</param>
        /// <returns>The combined filter.</returns>
        public ILogFilter Or(params Func<ILogEntry, bool>[] funcs) => Or(funcs as IEnumerable<Func<ILogEntry, bool>>);

        #endregion or

        /// <summary>
        ///     Inverts this filter (inverts the result of the <see cref="CanLog(ILogEntry)"/> method.
        /// </summary>
        /// <returns>The inverted filter.</returns>
        public ILogFilter Not() => new NotFilter(this);
    }
}
