using WSharp.Logging;
using WSharp.Logging.Loggers;

namespace WSharp.Tests.Mocks
{
    public class MockLogger : ALogger
    {
        public ILogEntry LastLog { get; set; }

        public override void InternalLog(ILogEntry logEntry) => LastLog = logEntry;
    }
}
