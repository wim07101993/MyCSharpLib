using System.Collections.Generic;

namespace WSharp.Logging.Loggers
{
    /// <summary>A logger that dispatches logs to other loggers.</summary>
    public interface ILogDispatcher : ICollection<ILogger>, ILogger
    {
    }
}
