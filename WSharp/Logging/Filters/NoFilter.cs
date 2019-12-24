namespace WSharp.Logging.Filters
{
    /// <summary>Blocks no logs.</summary>
    public class NoFilter : ALogFilter
    {
        /// <summary>Check whether a log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        public override bool CanLog(ILogEntry log) => true;
    }
}
