using MyCSharpLib.Services.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public class ReceivedEventArgs
    {
        private readonly IDeserializer _deserializer;
        

        public ReceivedEventArgs(IEnumerable<byte> content, IDeserializer deserializer)
        {
            _deserializer = deserializer;

            BytesContent = content;
        }


        public IEnumerable<byte> BytesContent { get; }
        public string StringContent
        {
            get
            {
                var byteArray = BytesContent.ToArray();
                return Encoding.ASCII.GetString(byteArray, 0, byteArray.Length);
            }
        }
        

        public async Task<T> GetDeserializedContent<T>(IDeserializer deserializer = null)
        {
            if (deserializer == null)
                deserializer = _deserializer;

            return await deserializer.DeserializeAsync<T>(StringContent);
        }
    }
}
