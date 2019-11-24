using Prism.Events;
using Prism.Mvvm;
using System.ComponentModel;
using WSharp.Logging.Loggers;

namespace WSharp
{
    public abstract class AViewModel : BindableBase, IViewModel
    {
        protected AViewModel()
        {
            EventAggregator = new EventAggregator();
            Logger = new MemoryLogger();
        }

        protected AViewModel(IEventAggregator eventAggregator, ILogger logger)
        {
            EventAggregator = eventAggregator;
            Logger = logger;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual IEventAggregator EventAggregator { get; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual ILogger Logger { get; }
    }
}
