using WSharp.Services.Serialization.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace WSharp.Services.Serialization
{
    /// <summary>
    /// Serializes an object of a given type to xml format.
    /// </summary>
    public class XmlSerializer : ISerializerDeserializer
    {
        /// <summary>
        /// Extension for the files with the serialized content. (.xml)
        /// </summary>
        public string FileExtension { get; } = "xml";


        /// <summary>
        /// Deserializes the xml-content that is received from a <see cref="TextReader"/> to type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the content to.</typeparam>
        /// <param name="reader">Reader to read the serialized object from.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(TextReader reader)
            => reader.DeserializeXml<T>();

        /// <summary>
        /// Deserializes the a xml-string to type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the string to.</typeparam>
        /// <param name="serializedValue">String to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(string serializedValue)
            => serializedValue.DeserializeXml<T>();

        /// <summary>
        /// Deserializes the xml-content that is received from a <see cref="TextReader"/> to type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the content to.</typeparam>
        /// <param name="reader">Reader to read the serialized object from.</param>
        /// <returns>The deserialized object.</returns>
        public async Task<T> DeserializeAsync<T>(TextReader reader)
            => await reader.DeserializeXmlAsync<T>();

        /// <summary>
        /// Deserializes the a xml-string to type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize the string to.</typeparam>
        /// <param name="serializedValue">String to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public async Task<T> DeserializeAsync<T>(string serializedValue)
            => await serializedValue.DeserializeXmlAsync<T>();


        /// <summary>
        /// Serializes an object with the xml format and writes its serialized content to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="value">Object to serialize.</param>
        /// <param name="writer">Writer to write the serialized value to.</param>
        public void Serialize(object value, TextWriter writer)
            => writer.SerializeXml(value);

        /// <summary>
        /// Serializes an object with the xml and writes its serialized content to a string.
        /// </summary>
        /// <param name="value">Object to serialize.</param>
        /// <returns>The serialized object.</returns>
        public string Serialize(object value)
            => value.SerializeXml();

        /// <summary>
        /// Serializes an object with the xml and writes its serialized content to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="value">Object to serialize.</param>
        /// <param name="writer">Writer to write the serialized value to.</param>
        public async Task SerializeAsync(object value, TextWriter writer)
            => await writer.SerializeXmlAsync(value);

        /// <summary>
        /// Serializes an object with the xml and writes its serialized content to a string.
        /// </summary>
        /// <param name="value">Object to serialize.</param>
        /// <returns>The serialized object.</returns>
        public async Task<string> SerializeAsync(object value)
            => await value.SerializeXmlAsync();
    }
}
