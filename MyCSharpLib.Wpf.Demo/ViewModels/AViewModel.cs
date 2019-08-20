using MyCSharpLib.Services.Logging.Loggers;
using MyCSharpLib.Wpf.Demo.ViewModelInterfaces;
using Prism.Mvvm;

namespace MyCSharpLib.Wpf.Demo.ViewModels
{
    public class AViewModel : BindableBase, IViewModel
    {
        public AViewModel(ILogDispatcher logger)
        {
            Logger = logger;
        }


        protected ILogDispatcher Logger { get; }
    }
}
