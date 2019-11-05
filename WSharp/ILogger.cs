using Prism.Events;
using System.ComponentModel;
using WSharp.Logging.Loggers;

namespace WSharp
{
    public interface IViewModel : INotifyPropertyChanged
    {
        IEventAggregator EventAggregator { get; }
        ILogger Logger { get; }
    }
}
