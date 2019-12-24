using System.IO;
using System.Threading.Tasks;

using WSharp.Extensions;

namespace WSharp.Serialization
{
    /// <summary>Serializes an object of a given type to json format.</summary>
    public class JsonSerializer : ISerializerDeserializer
    {
        /// <summary>Extension for the files with the serialized content. (.json)</summary>
        public string FileExtension { get; } = "json";

        /// <summary>
        ///     Deserializes the json-content that is received from a <see cref="TextReader"/> to
        ///     type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the content to.</typeparam>
        /// <param name="reader">Reader to read the serialized object from.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(TextReader reader)
            => reader.DeserializeJson<T>();

        /// <summary>Deserializes the a json-string to type <see cref="T"/>.</summary>
        /// <typeparam name="T">Type of object to deserialize the string to.</typeparam>
        /// <param name="serializedValue">String to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(string serializedValue)
            => serializedValue.DeserializeJson<T>();

        /// <summary>
        ///     Deserializes the json-content that is received from a <see cref="TextReader"/> to
        ///     type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the content to.</typeparam>
        /// <param name="reader">Reader to read the serialized object from.</param>
        /// <returns>The deserialized object.</returns>
        public async Task<T> DeserializeAsync<T>(TextReader reader)
            => await reader.DeserializeJsonAsync<T>();

        /// <summary>Deserializes the a json-string to type <see cref="T"/>.</summary>
        /// <typeparam name="T">Type of object to deserialize the string to.</typeparam>
        /// <param name="serializedValue">String to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public async Task<T> DeserializeAsync<T>(string serializedValue)
            => await serializedValue.DeserializeJsonAsync<T>();

        /// <summary>
        ///     Serializes an object with the json format and writes its serialized content to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="value">Object to serialize.</param>
        /// <param name="writer">Writer to write the serialized value to.</param>
        public void Serialize(object value, TextWriter writer)
            => writer.SerializeJson(value);

        /// <summary>
        ///     Serializes an object with the json and writes its serialized content to a string.
        /// </summary>
        /// <param name="value">Object to serialize.</param>
        /// <returns>The serialized object.</returns>
        public string Serialize(object value)
            => value.SerializeJson();

        /// <summary>
        ///     Serializes an object with the json and writes its serialized content to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="value">Object to serialize.</param>
        /// <param name="writer">Writer to write the serialized value to.</param>
        public async Task SerializeAsync(object value, TextWriter writer)
            => await writer.SerializeJsonAsync(value);

        /// <summary>
        ///     Serializes an object with the json and writes its serialized content to a string.
        /// </summary>
        /// <param name="value">Object to serialize.</param>
        /// <returns>The serialized object.</returns>
        public async Task<string> SerializeAsync(object value)
            => await value.SerializeJsonAsync();
    }
}
