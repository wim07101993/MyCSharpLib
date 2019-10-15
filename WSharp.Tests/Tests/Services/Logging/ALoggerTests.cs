using FluentAssertions;
using WSharp.Logging;
using WSharp.Logging.Filters;
using WSharp.Tests.Mocks;
using NUnit.Framework;
using System.Diagnostics;

namespace WSharp.Tests.Tests.Services.Logging
{
    public class ALoggerTests
    {
        [Test]
        public void ShouldLog()
        {
            var logger = new MockLogger { ShouldLog = false };
            var entry = new LogEntry();
            logger.Log(entry);

            logger.LastLog
                .Should()
                .BeNull("The logger should not have logged");

            logger.ShouldLog = true;
            logger.Log(entry);
            logger.LastLog.Id
                .Should()
                .Be(entry.Id, "that is the entry that was logged");
        }

        [Test]
        public void Filter()
        {
            var logger = new MockLogger { Filter = new BlockFilter() };

            var errorEntry = new LogEntry(eventType: TraceEventType.Error);
            var infoEntry = new LogEntry(eventType: TraceEventType.Information);
            var criticalEntry = new LogEntry(eventType: TraceEventType.Critical);

            logger.Log(errorEntry);
            logger.LastLog
                .Should()
                .BeNull("the filter blocks everything");
            logger.Log(infoEntry);
            logger.LastLog
                .Should()
                .BeNull("the filter blocks everything");
            logger.Log(criticalEntry);
            logger.LastLog
                .Should()
                .BeNull("the filter blocks everything");

            logger.Filter = new NoFilter();

            logger.Log(errorEntry);
            logger.LastLog.Id
                .Should()
                .Be(errorEntry.Id, "the filter now passes everything");
        }
    }
}
