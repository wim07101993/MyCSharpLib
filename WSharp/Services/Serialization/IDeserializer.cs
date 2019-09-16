using System.IO;
using System.Threading.Tasks;

namespace WSharp.Services.Serialization
{
    /// <summary>
    /// Deserializes a string to a given type.
    /// </summary>
    public interface IDeserializer
    {
        /// <summary>
        /// Extension for the files with the serialized content.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Deserializes the content that is received from a <see cref="TextReader"/> to type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the content to.</typeparam>
        /// <param name="reader">Reader to read the serialized object from.</param>
        /// <returns>The deserialized object.</returns>
        T Deserialize<T>(TextReader reader);

        /// <summary>
        /// Deserializes the a string to type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the string to.</typeparam>
        /// <param name="serializedValue">String to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        T Deserialize<T>(string serializedValue);

        /// <summary>
        /// Deserializes the content that is received from a <see cref="TextReader"/> to type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the content to.</typeparam>
        /// <param name="reader">Reader to read the serialized object from.</param>
        /// <returns>The deserialized object.</returns>
        Task<T> DeserializeAsync<T>(TextReader reader);

        /// <summary>
        /// Deserializes the a string to type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the string to.</typeparam>
        /// <param name="serializedValue">String to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        Task<T> DeserializeAsync<T>(string serializedValue);
    }
}
