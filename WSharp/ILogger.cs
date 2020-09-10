using System.ComponentModel;

using WSharp.Logging.Loggers;

namespace WSharp
{
    public interface IViewModel : INotifyPropertyChanged
    {
        ILogger Logger { get; }
    }
}
