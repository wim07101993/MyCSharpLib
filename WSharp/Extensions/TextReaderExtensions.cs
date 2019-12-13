using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Newtonsoft.Json;

namespace WSharp.Extensions
{
    /// <summary>Extension methods for the <see cref="TextReader"/> class.</summary>
    public static class TextReaderExtensions
    {
        /// <summary>
        ///     Deserializes the readers content to the type <typeparamref name="T"/> using json.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="reader">The reader to read the contents de deserialize from.</param>
        /// <returns>The deserialize value.</returns>
        public static T DeserializeJson<T>(this TextReader reader)
            => new JsonSerializer().Deserialize<T>(new JsonTextReader(reader));

        /// <summary>Deserializes the readers content to the given type using json.</summary>
        /// <param name="reader">The reader to read the contents de deserialize from.</param>
        /// <param name="objectType">The type to deserialize to.</param>
        /// <returns>The deserialize value.</returns>
        public static object DeserializeJson(this TextReader reader, Type objectType)
            => new JsonSerializer().Deserialize(reader, objectType);

        /// <summary>
        ///     Deserializes the readers content to the type <typeparamref name="T"/> using json.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="reader">The reader to read the contents de deserialize from.</param>
        /// <returns>The deserialize value.</returns>
        public static async Task<T> DeserializeJsonAsync<T>(this TextReader reader)
        {
            using var jsonReader = new JsonTextReader(reader);

            return await new JsonSerializer().DeserializeAsync<T>(jsonReader);
        }

        /// <summary>Deserializes the readers content to the given type using json.</summary>
        /// <param name="reader">The reader to read the contents de deserialize from.</param>
        /// <param name="objectType">The type to deserialize to.</param>
        /// <returns>The deserialize value.</returns>
        public static Task<object> DeserializeJsonAsync(this TextReader reader, Type objectType)
            => new JsonSerializer().DeserializeAsync(reader, objectType);

        /// <summary>
        ///     Deserializes the readers content to the type <typeparamref name="T"/> using xml.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="reader">The reader to read the contents de deserialize from.</param>
        /// <returns>The deserialize value.</returns>
        public static T DeserializeXml<T>(this TextReader reader)
            => new XmlSerializer(typeof(T)).Deserialize<T>(reader);

        /// <summary>Deserializes the readers content to the given type using xml.</summary>
        /// <param name="reader">The reader to read the contents de deserialize from.</param>
        /// <param name="objectType">The type to deserialize to.</param>
        /// <returns>The deserialize value.</returns>
        public static object DeserializeXml(this TextReader reader, Type objectType)
            => new XmlSerializer(objectType).Deserialize(reader);

        /// <summary>
        ///     Deserializes the readers content to the type <typeparamref name="T"/> using xml.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="reader">The reader to read the contents de deserialize from.</param>
        /// <returns>The deserialize value.</returns>
        public static async Task<T> DeserializeXmlAsync<T>(this TextReader reader)
            => await new XmlSerializer(typeof(T)).DeserializeAsync<T>(reader);

        /// <summary>Deserializes the readers content to the given type using xml.</summary>
        /// <param name="reader">The reader to read the contents de deserialize from.</param>
        /// <param name="objectType">The type to deserialize to.</param>
        /// <returns>The deserialize value.</returns>
        public static async Task<object> DeserializeXmlAsync(this TextReader reader, Type objectType)
            => await new XmlSerializer(objectType).DeserializeAsync(reader);
    }
}
