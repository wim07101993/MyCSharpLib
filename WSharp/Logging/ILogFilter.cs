namespace WSharp.Logging
{
    /// <summary>Determines whether a log entry should be logged or not.</summary>
    public interface ILogFilter
    {
        /// <summary>Check whether a log should be logged.</summary>
        /// <param name="log">Log to log.</param>
        /// <returns>Whether a log should be logged.</returns>
        bool FilterLog(ILogEntry log);
    }
}