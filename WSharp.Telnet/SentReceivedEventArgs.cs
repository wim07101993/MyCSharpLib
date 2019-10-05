using WSharp.Extensions;
using WSharp.Services.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WSharp.Telnet
{
    public class SentReceivedEventArgs
    {
        private readonly IDeserializer _deserializer;
        

        public SentReceivedEventArgs(IEnumerable<byte> content, IDeserializer deserializer)
        {
            _deserializer = deserializer;

            BytesContent = content;
        }


        public IEnumerable<byte> BytesContent { get; }
        public string StringContent => BytesContent.ToAsciiString();
        

        public async Task<T> GetDeserializedContent<T>(IDeserializer deserializer = null)
        {
            if (deserializer == null)
                deserializer = _deserializer;

            return await deserializer.DeserializeAsync<T>(StringContent);
        }
    }
}
