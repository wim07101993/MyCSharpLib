using MyCSharpLib.Services.Logging.Loggers;
using MyCSharpLib.Wpf.Demo.Strings;
using MyCSharpLib.Wpf.Demo.ViewModelInterfaces;
using Prism.Mvvm;
using System.ComponentModel;

namespace MyCSharpLib.Wpf.Demo.ViewModels
{
    public class AViewModel : BindableBase, IViewModel
    {
        public AViewModel(ApplicationStrings strings, ILogDispatcher logger)
        {
            Strings = strings;
            Logger = logger;
        }


        [Browsable(false)]
        public ApplicationStrings Strings { get; }
        protected ILogDispatcher Logger { get; }
    }
}
