using System.IO;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Serialization
{
    public interface ISerializer
    {
        string FileExtension { get; }

        void Serialize(object value, TextWriter writer);
        string Serialize(object value);

        Task SerializeAsync(object value, TextWriter writer);
        Task<string> SerializeAsync(object value);
    }
}
