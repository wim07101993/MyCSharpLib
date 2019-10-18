using WSharp.Logging;
using WSharp.Logging.Loggers;

namespace WSharp.Tests.Mocks
{
    public class MockLogger : ALogger
    {
        public ILogEntry LastLog { get; set; }

        protected override void InternalLog(ILogEntry logEntry) => LastLog = logEntry;
    }
}
