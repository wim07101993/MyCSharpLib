using System;
using System.Threading.Tasks;

namespace WSharp.Logging.Loggers
{
    /// <summary>Interface that describes a logger.</summary>
    public interface ILogger : IDisposable
    {
        /// <summary>A lock to synchronize tasks/threads.</summary>
        object GlobalLock { get; }

        /// <summary>If <see langword="false"/>: no logs are logged.</summary>
        bool ShouldLog { get; set; }

        /// <summary>Filter that filters what logs should be logged.</summary>
        ILogFilter Filter { get; set; }

        /// <summary>
        ///     A buffer to write to and in the end log with the <see cref="Log(IBufferLogEntry)"/>
        ///     or <see cref="LogBuffer()"/> method.
        /// </summary>
        IBufferLogEntry Buffer { get; }

        /// <summary>Logs the given entry if it passes the logfilter.</summary>
        /// <param name="logEntry">Log entry to log.``</param>
        void Log(ILogEntry logEntry);

        /// <summary>Logs the <see cref="Buffer"/>.</summary>
        void LogBuffer();

        /// <summary>Logs the given entry if it passes the logfilter.</summary>
        /// <param name="logEntry">Log entry to log.``</param>
        Task LogAsync(ILogEntry logEntry);

        /// <summary>Logs the <see cref="Buffer"/>.</summary>
        Task LogBufferAsync();

        /// <summary>Releases all resources.</summary>
        /// <param name="isDisposing">
        ///     Indicates whether the <see cref="Dispose"/> method has been called before.
        /// </param>
        void Dispose(bool isDisposing);
    }
}
