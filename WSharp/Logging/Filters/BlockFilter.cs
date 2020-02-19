namespace WSharp.Logging.Filters
{
    /// <summary>Blocks all logs.</summary>
    public class BlockFilter : ALogFilter
    {
        /// <summary>Check whether a log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        public override bool CanLog(ILogEntry log) => false;
    }
}
