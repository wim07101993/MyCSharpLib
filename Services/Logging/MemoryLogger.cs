using System.Threading.Tasks;

namespace MyCSharpLib.Services.Logging
{
    public class MemoryLogger : ATraceListener
    {
        public override Task WriteAsync(string message)
        {
            
        }

        public ObservableCollection<string> 
    }
}
