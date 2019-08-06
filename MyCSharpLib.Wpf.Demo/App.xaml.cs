using MyCSharpLib.Services;
using MyCSharpLib.Services.Logging;
using MyCSharpLib.Services.Logging.Loggers;
using MyCSharpLib.Services.Serialization;
using MyCSharpLib.Wpf.Controls.Demo.Views;
using MyCSharpLib.Wpf.Demo.Properties;
using MyCSharpLib.Wpf.Demo.Strings;
using MyCSharpLib.Wpf.Demo.ViewModelInterfaces;
using MyCSharpLib.Wpf.Demo.ViewModels;
using System.Windows;
using Unity;

namespace MyCSharpLib.Wpf.Controls.Demo
{
    public partial class App
    {
        public static IUnityContainer UnityContainer { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitSettings();
            InitUnityContainer();
            InitLogging();

            MainWindow = UnityContainer.Resolve<MainWindow>();
            MainWindow.Show();
            UnityContainer.Resolve<ILogDispatcher>().Log("Application", "Show main window");
        }

        private void InitSettings()
        {
            if (Wpf.Demo.Properties.Settings.Default.StringsSettings == null)
            {
                Wpf.Demo.Properties.Settings.Default.StringsSettings = new StringsFileSettings
                {
                    Language = "English",
                    LanguagesDirectory = $@"languages\"
                };

                Wpf.Demo.Properties.Settings.Default.Save();
            }
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
                .RegisterSingleton<IStringsProvider, StringsProvider<ApplicationStrings>>()
                .RegisterSingleton<IStringsProvider<ApplicationStrings>, StringsProvider<ApplicationStrings>>()
                .RegisterSingleton<ILogDispatcherFactory, LogDispatcherFactory>()
                .RegisterFactory<ILogDispatcher>(x =>
                {
                    var factory = x.Resolve<ILogDispatcherFactory>();
                    return factory.Resolve();
                })
                // view models
                .RegisterType<ILoggingViewModel, LoggingViewModel>()
                .RegisterType<IMainWindowViewModel, MainWindowViewModel>();

            var settings = Settings.Default.StringsSettings;
            UnityContainer
                .RegisterInstance(settings)
                .RegisterInstance<IStringsFileSettings>(settings);

            var strings = UnityContainer.Resolve<IStringsProvider<ApplicationStrings>>().Strings;
            UnityContainer
                .RegisterInstance(strings);
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
