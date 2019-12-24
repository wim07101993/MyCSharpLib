using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WSharp.Logging
{
    /// <summary>Determines whether a log entry should be logged or not.</summary>
    public interface ILogFilter
    {
        /// <summary>Check whether a log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        bool CanLog(ILogEntry log);

        /// <summary>Combines this filter with another one in an AND relation.</summary>
        /// <param name="filter">This filter.</param>
        /// <param name="filterToAdd">Filter to AND with these filters.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter And(IEnumerable<ILogFilter> filtersToAnd);

        /// <summary>Combines this filter with another one in an AND relation.</summary>
        /// <param name="filter">This filter.</param>
        /// <param name="filtersToAdd">Filter to AND with these filters.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter And(params ILogFilter[] filtersToAnd);

        /// <summary>
        ///     Combines this filter with other event types in an AND relation. This filter should
        ///     accept the log and all the given event types should be present.
        /// </summary>
        /// <param name="eventTypes">Filters to and.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter And(IEnumerable<TraceEventType> eventType);

        /// <summary>
        ///     Combines this filter with other event types in an AND relation. This filter should
        ///     accept the log and all the given event types should be present.
        /// </summary>
        /// <param name="eventTypes">Filters to and.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter And(params TraceEventType[] eventType);

        /// <summary>
        ///     Combines this filter with other funcs in an AND relation. This filter should accept
        ///     the log if all the funcs agree.
        /// </summary>
        /// <param name="funcs">Funcs to and.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter And(IEnumerable<Func<ILogEntry, bool>> funcs);

        /// <summary>
        ///     Combines this filter with other funcs in an AND relation. This filter should accept
        ///     the log if all the funcs agree.
        /// </summary>
        /// <param name="funcs">Funcs to and.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter And(params Func<ILogEntry, bool>[] funcs);

        /// <summary>Combines this filter with another one in an OR relation.</summary>
        /// <param name="filter">This filter.</param>
        /// <param name="filtersToAdd">Filter to OR with these filters.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter Or(IEnumerable<ILogFilter> filtersToOr);

        /// <summary>Combines this filter with another one in an OR relation.</summary>
        /// <param name="filter">This filter.</param>
        /// <param name="filtersToAdd">Filter to OR with these filters.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter Or(params ILogFilter[] filtersToOr);

        /// <summary>
        ///     Combines this filter with other event types in an OR relation. This filter should
        ///     accept the log or any of the given event types should be present.
        /// </summary>
        /// <param name="eventTypes">Event types to or.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter Or(IEnumerable<TraceEventType> eventType);

        /// <summary>
        ///     Combines this filter with other event types in an OR relation. This filter should
        ///     accept the log or any of the given event types should be present.
        /// </summary>
        /// <param name="eventTypes">Event types to or.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter Or(params TraceEventType[] eventType);

        /// <summary>
        ///     Combines this filter with other funcs in an OR relation. This filter should accept
        ///     the log if any the funcs agree.
        /// </summary>
        /// <param name="funcs">Funcs to and.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter Or(IEnumerable<Func<ILogEntry, bool>> funcs);

        /// <summary>
        ///     Combines this filter with other funcs in an OR relation. This filter should accept
        ///     the log if any the funcs agree.
        /// </summary>
        /// <param name="funcs">Funcs to and.</param>
        /// <returns>The combined filter.</returns>
        ILogFilter Or(params Func<ILogEntry, bool>[] funcs);

        /// <summary>
        ///     Inverts this filter (inverts the result of the <see cref="CanLog(ILogEntry)"/> method.
        /// </summary>
        /// <returns>The inverted filter.</returns>
        ILogFilter Not();
    }
}
