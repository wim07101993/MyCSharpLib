using System.IO;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Serialization
{
    public interface IDeserializer
    {
        string FileExtension { get; }

        T Deserialize<T>(TextReader reader);
        T Deserialize<T>(string serializedValue);

        Task<T> DeserializeAsync<T>(TextReader reader);
        Task<T> DeserializeAsync<T>(string serializedValue);
    }
}
