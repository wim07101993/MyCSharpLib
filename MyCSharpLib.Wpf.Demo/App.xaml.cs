using MyCSharpLib.Services.Logging;
using MyCSharpLib.Services.Logging.Loggers;
using MyCSharpLib.Services.Serialization;
using MyCSharpLib.Wpf.Controls.Demo.Views;
using MyCSharpLib.Wpf.Demo.ViewModelInterfaces;
using MyCSharpLib.Wpf.Demo.ViewModels;
using MyCSharpLib.Extensions;
using System.Threading;
using System.Windows;
using Unity;

namespace MyCSharpLib.Wpf.Controls.Demo
{
    public partial class App : IWithUnityContainer
    {
        public IUnityContainer UnityContainer { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.SetLanguage();

            InitSettings();
            InitUnityContainer();
            InitLogging();

            base.OnStartup(e);
          
            MainWindow = UnityContainer.Resolve<MainWindow>();
            MainWindow.Show();
            UnityContainer.Resolve<ILogDispatcher>().Log("Application", "Show main window");
        }

        private void InitSettings()
        {
        }

        private void InitUnityContainer()
        {
            UnityContainer = new UnityContainer()
#if DEBUG
                .EnableDiagnostic()
#endif
                // services
                .RegisterType<ISerializerDeserializer, JsonSerializer>()
                .RegisterType<ISerializer, JsonSerializer>()
                .RegisterType<IDeserializer, JsonSerializer>()
                .RegisterSingleton<ILogDispatcherFactory, LogDispatcherFactory>()
                .RegisterFactory<ILogDispatcher>(x =>
                {
                    var factory = x.Resolve<ILogDispatcherFactory>();
                    return factory.Resolve();
                })
                // view models
                .RegisterType<ILoggingViewModel, LoggingViewModel>()
                .RegisterType<ObjectBrowserViewModel>()
                .RegisterType<IMainWindowViewModel, MainWindowViewModel>();
        }

        private void InitLogging()
        {
            var factory = UnityContainer.Resolve<ILogDispatcherFactory>();
            factory
                .RegisterLogDispatcherType<LogDispatcher>()
                .RegisterSingleton<IMemoryLogger, MemoryLogger>()
                .RegisterSingleton<IConsoleLogger, ConsoleLogger>();
        }

    }
}
