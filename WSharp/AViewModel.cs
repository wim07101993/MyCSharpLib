using System.ComponentModel;

using WSharp.Logging.Loggers;
using WSharp.Observables;

namespace WSharp
{
    public abstract class AViewModel : ObservableObject, IViewModel
    {
        protected AViewModel()
        {
            Logger = new MemoryLogger();
        }

        protected AViewModel(ILogger logger)
        {
            Logger = logger;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual ILogger Logger { get; }
    }
}
