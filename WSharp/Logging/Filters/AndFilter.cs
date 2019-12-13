using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WSharp.Logging.Filters
{
    /// <summary>Filter that represents an AND relation.</summary>
    public class AndFilter : ALogFilter
    {
        /// <summary> Constructs a new instance of the <see cref="AndFilter"/>. All the event types
        /// should be present in the log to pass. ((log.EventType & eventType) > 0).</summary>
        /// <param name="eventTypes">Eventtypes that should be present in the log.</param>
        public AndFilter(params TraceEventType[] eventTypes)
            : this(eventTypes as IEnumerable<TraceEventType>)
        {
        }

        /// <summary> Constructs a new instance of the <see cref="AndFilter"/>. All the event types
        /// should be present in the log to pass. ((log.EventType & eventType) > 0).</summary>
        /// <param name="eventTypes">Eventtypes that should be present in the log.</param>
        public AndFilter(IEnumerable<TraceEventType> eventTypes)
        {
            Filters = eventTypes.Select(e => new Func<ILogEntry, bool>(log => (log.EventType & e) > 0));
        }

        /// <summary>
        ///     Constructs a new instance of the <see cref="AndFilter"/>. All the filters should
        ///     agree on logging the log. (fitlers.All(x =&gt; x.CanLog(log)).
        /// </summary>
        /// <param name="filters">Filters that should agree on logging.</param>
        public AndFilter(params ILogFilter[] filters)
            : this(filters as IEnumerable<ILogFilter>)
        {
        }

        /// <summary>
        ///     Constructs a new instance of the <see cref="AndFilter"/>. All the filters should
        ///     agree on logging the log. (fitlers.All(x =&gt; x.CanLog(log)).
        /// </summary>
        /// <param name="filters">Filters that should agree on logging.</param>
        public AndFilter(IEnumerable<ILogFilter> filters)
        {
            Filters = filters.Select(f => new Func<ILogEntry, bool>(f.CanLog));
        }

        /// <summary>
        ///     Constructs a new instance of the <see cref="AndFilter"/>. All the funcs should agree
        ///     on logging the log. (fitlers.All(x =&gt; x(log)).
        /// </summary>
        /// <param name="funcs">Funcs that should agree on logging.</param>
        public AndFilter(params Func<ILogEntry, bool>[] funcs)
            : this(funcs as IEnumerable<Func<ILogEntry, bool>>)
        {
        }

        /// <summary>
        ///     Constructs a new instance of the <see cref="AndFilter"/>. All the funcs should agree
        ///     on logging the log. (fitlers.All(x =&gt; x(log)).
        /// </summary>
        /// <param name="funcs">Funcs that should agree on logging.</param>
        public AndFilter(IEnumerable<Func<ILogEntry, bool>> funcs)
        {
            Filters = funcs;
        }

        /// <summary>Filters to use in the AND relation.</summary>
        protected IEnumerable<Func<ILogEntry, bool>> Filters { get; }

        /// <summary>Check whether a log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        public override bool CanLog(ILogEntry log) => Filters.All(x => x(log));
    }
}
