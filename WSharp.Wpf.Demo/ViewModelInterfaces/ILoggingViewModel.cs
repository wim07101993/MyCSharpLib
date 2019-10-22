using WSharp.Logging;
using System.Collections.ObjectModel;

namespace WSharp.Wpf.Demo.ViewModelInterfaces
{
    public interface ILoggingViewModel : IViewModel
    {
        ReadOnlyObservableCollection<ILogEntry> Logs { get; }
    }
}
