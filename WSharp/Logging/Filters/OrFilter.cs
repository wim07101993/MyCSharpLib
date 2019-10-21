using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WSharp.Logging.Filters
{
    /// <summary>Filter that represents an OR relation.</summary>
    public class OrFilter : ALogFilter
    {
        /// <summary>
        ///     Constructs a new instance of the <see cref="OrFilter"/>. Any of the event types
        ///     should be present in the log to pass. ((log.EventType | eventType) &gt; 0).
        /// </summary>
        /// <param name="eventTypes">Eventtypes that should be present in the log.</param>
        public OrFilter(params TraceEventType[] eventTypes)
            : this(eventTypes as IEnumerable<TraceEventType>)
        {
        }

        /// <summary>
        ///     Constructs a new instance of the <see cref="OrFilter"/>. Any of the event types
        ///     should be present in the log to pass. ((log.EventType | eventType) &gt; 0).
        /// </summary>
        /// <param name="eventTypes">Eventtypes that should be present in the log.</param>
        public OrFilter(IEnumerable<TraceEventType> eventTypes)
        {
            Filters = eventTypes.Select(e => new Func<ILogEntry, bool>(log => (log.EventType | e) > 0));
        }

        /// <summary>
        ///     Constructs a new instance of the <see cref="OrFilter"/>. Any the filters should agree
        ///     on logging the log. (fitlers.Any(x =&gt; x.CanLog(log)).
        /// </summary>
        /// <param name="filters">Filters that should agree on logging.</param>
        public OrFilter(params ILogFilter[] filters)
            : this(filters as IEnumerable<ILogFilter>)
        {
        }

        /// <summary>
        ///     Constructs a new instance of the <see cref="OrFilter"/>. Any of the filters should
        ///     agree on logging the log. (fitlers.Any(x =&gt; x.CanLog(log)).
        /// </summary>
        /// <param name="filters">Filters that should agree on logging.</param>
        public OrFilter(IEnumerable<ILogFilter> filters)
        {
            Filters = filters.Select(f => new Func<ILogEntry, bool>(f.CanLog));
        }

        /// <summary>
        ///     Constructs a new instance of the <see cref="OrFilter"/>. Any of the funcs should
        ///     agree on logging the log. (fitlers.Any(x =&gt; x(log)).
        /// </summary>
        /// <param name="funcs">Funcs that should agree on logging.</param>
        public OrFilter(params Func<ILogEntry, bool>[] funcs)
            : this(funcs as IEnumerable<Func<ILogEntry, bool>>)
        {
        }

        /// <summary>
        ///     Constructs a new instance of the <see cref="OrFilter"/>. Any of the funcs should
        ///     agree on logging the log. (fitlers.Any(x =&gt; x(log)).
        /// </summary>
        /// <param name="funcs">Funcs that should agree on logging.</param>
        public OrFilter(IEnumerable<Func<ILogEntry, bool>> funcs)
        {
            Filters = funcs;
        }

        /// <summary>Filters to use in the AND relation.</summary>
        protected IEnumerable<Func<ILogEntry, bool>> Filters { get; }

        /// <summary>Check whether a log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        public override bool CanLog(ILogEntry log) => Filters.Any(x => x(log));
    }
}