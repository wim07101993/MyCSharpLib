using WSharp.Logging;
using WSharp.Logging.Loggers;
using WSharp.Serialization;
using WSharp.Wpf.Controls.Demo.Views;
using WSharp.Wpf.Demo.ViewModelInterfaces;
using WSharp.Wpf.Demo.ViewModels;
using WSharp.Languages;
using System.Threading;
using System.Windows;
using Unity;

namespace WSharp.Wpf.Controls.Demo
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
