using System.Collections.Generic;

namespace WSharp.Logging.Loggers
{
    public interface ILogDispatcher : ICollection<ILogger>, ILogger
    {
    }
}