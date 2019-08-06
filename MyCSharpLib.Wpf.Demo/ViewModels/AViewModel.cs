using MyCSharpLib.Services.Logging.Loggers;
using MyCSharpLib.Wpf.Demo.Strings;
using MyCSharpLib.Wpf.Demo.ViewModelInterfaces;
using Prism.Mvvm;

namespace MyCSharpLib.Wpf.Demo.ViewModels
{
    public class AViewModel : BindableBase, IViewModel
    {
        public AViewModel(ApplicationStrings strings, ILogDispatcher logger)
        {
            Strings = strings;
            Logger = logger;
        }


        public ApplicationStrings Strings { get; }
        protected ILogDispatcher Logger { get; }
    }
}
