using NUnit.Framework;
using Unity;
using WSharp.Extensions;
using WSharp.Files;
using WSharp.Logging;
using WSharp.Logging.Loggers;

namespace WSharp.Tests.Tests.Logging
{
    [TestFixture]
    public class LogDispatcherFactoryTests
    {
        [Test]
        public void TestResolve()
        {
            var container = new UnityContainer()
                .EnableDiagnostic()
                .RegisterWSharp()
                .RegisterInstance<IFileServiceSettings>(new FileServiceSettings("WSharp", "Test"))
                .RegisterSingleton<ILogger, ILogDispatcher>();
            
            container.Resolve<ILogDispatcherFactory>()
                .RegisterSingleton<IFileLogger, FileLogger>();

            var logger = container.Resolve<ILogger>();
        }
    }
}
