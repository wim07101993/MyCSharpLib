using MyCSharpLib.Services.Serialization.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Serialization
{
    public class JsonSerializer : ISerializer, IDeserializer
    {
        public string FileExtension { get; } = "json";


        public T Deserialize<T>(TextReader reader)
            => reader.DeserializeJson<T>();

        public T Deserialize<T>(string serializedValue)
            => serializedValue.DeserializeJson<T>();

        public async Task<T> DeserializeAsync<T>(TextReader reader)
            => await reader.DeserializeJsonAsync<T>();

        public async Task<T> DeserializeAsync<T>(string serializedValue)
            => await serializedValue.DeserializeJsonAsync<T>();


        public void Serialize(object value, TextWriter writer)
            => writer.SerializeJson(value);

        public string Serialize(object value)
            => value.SerializeJson();

        public async Task SerializeAsync(object value, TextWriter writer)
            => await writer.SerializeJsonAsync(value);

        public async Task<string> SerializeAsync(object value)
            => await value.SerializeJsonAsync();
    }
}
