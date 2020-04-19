using System.ComponentModel;

using Prism.Events;

using WSharp.Logging.Loggers;

namespace WSharp
{
    public interface IViewModel : INotifyPropertyChanged
    {
        IEventAggregator EventAggregator { get; }
        ILogger Logger { get; }
    }
}
