using System;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Logging
{
    public class ConsoleLogger : ALogger
    {
        private readonly object _lock = new object();


        private ConsoleLogger()
        {
        }


        public static ConsoleLogger Instance { get; } = new ConsoleLogger();


        public override Task WriteAsync(string message)
        {
            return Task.Factory.StartNew(() =>
            {
                lock (_lock)
                {
                    Console.Write(message);
                }
            });
        }
    }
}
