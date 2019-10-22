using WSharp.Logging.Loggers;
using WSharp.Wpf.Demo.ViewModelInterfaces;
using Prism.Mvvm;

namespace WSharp.Wpf.Demo.ViewModels
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
