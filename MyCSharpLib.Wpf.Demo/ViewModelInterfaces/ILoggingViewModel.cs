using MyCSharpLib.Services.Logging;
using System.Collections.ObjectModel;

namespace MyCSharpLib.Wpf.Demo.ViewModelInterfaces
{
    public interface ILoggingViewModel : IViewModel
    {
        ReadOnlyObservableCollection<ILogEntry> Entries { get; }
    }
}
