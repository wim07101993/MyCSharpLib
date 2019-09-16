using System;

namespace WSharp.Services.Logging.Loggers
{
    public class ConsoleLogger : TextWriterLogger, IConsoleLogger
    {
        public ConsoleLogger()
            : base(Console.Out)
        {
        }

        protected override bool EnsureWriter()
        {
            if (!IsWriterNull)
                return true;

            Writer = Console.Out;
            return true;
        }
    }
}
