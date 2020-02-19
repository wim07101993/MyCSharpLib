namespace WSharp.Logging.Filters
{
    /// <summary>
    ///     Filter that inverts the result of the <see cref="CanLog(ILogEntry)"/> method of another filter.
    /// </summary>
    public class NotFilter : ALogFilter
    {
        /// <summary>
        ///     Constructs a new instance of the <see cref="NotFilter"/>. It inverts the result of
        ///     the <see cref="CanLog(ILogEntry)"/> method.
        /// </summary>
        /// <param name="filter"></param>
        public NotFilter(ILogFilter filter)
        {
            Filter = filter;
        }

        /// <summary>Filter to invert.</summary>
        protected ILogFilter Filter { get; }

        /// <summary>
        ///     Check whether a log should be logged. (The inverted value of the original filter.)
        /// </summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        public override bool CanLog(ILogEntry log) => !Filter.CanLog(log);
    }
}
