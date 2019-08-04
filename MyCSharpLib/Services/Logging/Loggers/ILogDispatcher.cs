using System.Collections.Generic;

namespace MyCSharpLib.Services.Logging.Loggers
{
    public interface ILogDispatcher : ICollection<ILogger>, ILogger
    {
    }
}
