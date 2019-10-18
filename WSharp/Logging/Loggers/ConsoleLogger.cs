using System;

namespace WSharp.Logging.Loggers
{
    /// <summary>Logs to the <see cref="Console"/>.</summary>
    public class ConsoleLogger : TextWriterLogger, IConsoleLogger
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ConsoleLogger"/>.
        /// </summary>
        public ConsoleLogger()
            : base(Console.Out)
        {
        }

        /// <summary>
        /// Ensures a writer is present to log to.
        /// </summary>
        /// <returns>Indicates whether the writer is present.</returns>
        protected override bool EnsureWriter()
        {
            if (!IsWriterNull)
                return true;

            Writer = Console.Out;
            return true;
        }
    }
}