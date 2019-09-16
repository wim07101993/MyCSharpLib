using System.Collections.Generic;

namespace WSharp.Services.Logging.Loggers
{
    public interface ILogDispatcher : ICollection<ILogger>, ILogger
    {
    }
}
