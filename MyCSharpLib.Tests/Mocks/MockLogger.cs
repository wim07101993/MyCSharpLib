using MyCSharpLib.Services.Logging;
using MyCSharpLib.Services.Logging.Loggers;

namespace MyCSharpLib.Tests.Mocks
{
    public class MockLogger : ALogger
    {
        public ILogEntry LastLog { get; set; }

        public override void InternalLog(ILogEntry logEntry) => LastLog = logEntry;
    }
}
