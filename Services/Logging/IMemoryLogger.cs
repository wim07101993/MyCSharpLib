using System.Collections.ObjectModel;

namespace MyCSharpLib.Services.Logging
{
    public interface IMemoryLogger : ILogger
    {
        ObservableCollection<string> Logs { get; }
    }
}
