using MyCSharpLib.Services.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Telnet
{
    public class ReceivedEventArgs
    {
        private readonly IDeserializer _deserializer;


        public ReceivedEventArgs(byte[] content, IDeserializer deserializer)
        {
            _deserializer = deserializer;

            BytesContent = content;
        }


        public byte[] BytesContent { get; }
        public string StringContent => Encoding.ASCII.GetString(BytesContent, 0, BytesContent.Length);
        

        public async Task<T> GetDeserializedContent<T>(IDeserializer deserializer = null)
        {
            if (deserializer == null)
                deserializer = _deserializer;

            return await deserializer.DeserializeAsync<T>(StringContent);
        }
    }
}
