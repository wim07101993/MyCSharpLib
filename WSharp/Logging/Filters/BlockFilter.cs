namespace WSharp.Logging.Filters
{
    /// <summary>Blocks all logs.</summary>
    public class BlockFilter : ILogFilter
    {
        /// <summary>Check whether a log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        public bool FilterLog(ILogEntry log) => false;
    }
}