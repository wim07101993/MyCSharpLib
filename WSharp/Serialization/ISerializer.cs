using System.IO;
using System.Threading.Tasks;

namespace WSharp.Serialization
{
    /// <summary>Serializes an object of a given type.</summary>
    public interface ISerializer
    {
        /// <summary>Extension for the files with the serialized content.</summary>
        string FileExtension { get; }

        /// <summary>Serializes an object and writes its serialized content to a <see cref="TextWriter"/>.</summary>
        /// <param name="value">Object to serialize.</param>
        /// <param name="writer">Writer to write the serialized value to.</param>
        void Serialize(object value, TextWriter writer);

        /// <summary>Serializes an object and writes its serialized content to a string.</summary>
        /// <param name="value">Object to serialize.</param>
        /// <returns>The serialized object.</returns>
        string Serialize(object value);

        /// <summary>Serializes an object and writes its serialized content to a <see cref="TextWriter"/>.</summary>
        /// <param name="value">Object to serialize.</param>
        /// <param name="writer">Writer to write the serialized value to.</param>
        Task SerializeAsync(object value, TextWriter writer);

        /// <summary>Serializes an object and writes its serialized content to a string.</summary>
        /// <param name="value">Object to serialize.</param>
        /// <returns>The serialized object.</returns>
        Task<string> SerializeAsync(object value);
    }
}