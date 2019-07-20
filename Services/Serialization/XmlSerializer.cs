using MyCSharpLib.Services.Serialization.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Serialization
{
    public class XmlSerializer : ISerializer, IDeserializer
    {
        public string FileExtension { get; } = "xml";


        public T Deserialize<T>(TextReader reader)
            => reader.DeserializeXml<T>();

        public T Deserialize<T>(string serializedValue)
            => serializedValue.DeserializeXml<T>();

        public async Task<T> DeserializeAsync<T>(TextReader reader)
            => await reader.DeserializeXmlAsync<T>();

        public async Task<T> DeserializeAsync<T>(string serializedValue)
            => await serializedValue.DeserializeXmlAsync<T>();


        public void Serialize(object value, TextWriter writer)
            => writer.SerializeXml(value);

        public string Serialize(object value)
            => value.SerializeXml();

        public async Task SerializeAsync(object value, TextWriter writer)
            => await writer.SerializeXmlAsync(value);

        public async Task<string> SerializeAsync(object value)
            => await value.SerializeXmlAsync();
    }
}
