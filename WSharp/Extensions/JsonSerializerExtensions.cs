using System;
using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace WSharp.Extensions
{
    /// <summary>Extension metehods for the <see cref="JsonSerializer"/> class.</summary>
    public static class JsonSerializerExtensions
    {
        /// <summary>
        ///     Serializes the specified <see cref="object"/> and writes the JSON structure using
        ///     the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="serializer">The serializer to serialize with.</param>
        /// <param name="textWriter">The <see cref="TextWriter"/> used to write the JSON structure.</param>
        /// <param name="value">The <see cref="object"/> to serialize.</param>
        /// <param name="objectType">
        ///     The type of the value being serialized. This parameter is used when
        ///     <see cref="TypeNameHandling"/> is Auto to write out the type name if the type of the
        ///     value does not match. Specifying the type is optional.
        /// </param>
        public static Task SerializeAsync(this JsonSerializer serializer, TextWriter textWriter, object value, Type objectType = default)
        {
            return objectType == default
                ? Task.Factory.StartNew(() => serializer.Serialize(textWriter, value))
                : Task.Factory.StartNew(() => serializer.Serialize(textWriter, value, objectType));
        }

        /// <summary>
        ///     Serializes the specified <see cref="object"/> and writes the JSON structure using
        ///     the specified <see cref="JsonWriter"/>.
        /// </summary>
        /// <param name="serializer">The serializer to serialize with.</param>
        /// <param name="jsonWriter">The <see cref="JsonWriter"/> used to write the JSON structure.</param>
        /// <param name="value">The <see cref="object"/> to serialize.</param>
        /// <param name="objectType">
        ///     The type of the value being serialized. This parameter is used when
        ///     <see cref="TypeNameHandling"/> is Auto to write out the type name if the type of the
        ///     value does not match. Specifying the type is optional.
        /// </param>
        public static Task SerializeAsync(this JsonSerializer serializer, JsonWriter jsonWriter, object value, Type objectType = default)
        {
            return objectType == default
                ? Task.Factory.StartNew(() => serializer.Serialize(jsonWriter, value))
                : Task.Factory.StartNew(() => serializer.Serialize(jsonWriter, value, objectType));
        }

        /// <summary>Deserializes the JSON structure contained by the specified <see cref="JsonReader"/>.</summary>
        /// <param name="serializer">The serializer to deserialize with.</param>
        /// <param name="reader">
        ///     The <see cref="JsonReader"/> that contains the JSON structure to deserialize.
        /// </param>
        /// <returns>The <see cref="object"/> being deserialized.</returns>
        public static Task<object> DeserializeAsync(this JsonSerializer serializer, JsonTextReader reader)
            => Task.Factory.StartNew(() => serializer.Deserialize(reader));

        /// <summary>
        ///     Deserializes the JSON structure contained by the specified <see cref="JsonReader"/>
        ///     into an instance of the specified type.
        /// </summary>
        /// <param name="serializer">The serializer to deserialize with.</param>
        /// <param name="reader">The <see cref="JsonReader"/> containing the object.</param>
        /// <returns>The instance of <see cref="T"/> being deserialized.</returns>
        public static Task<T> DeserializeAsync<T>(this JsonSerializer serializer, JsonTextReader reader)
            => Task.Factory.StartNew(() => serializer.Deserialize<T>(reader));

        /// <summary>
        ///     Deserializes the JSON structure contained by the specified <see cref="JsonReader"/>
        ///     into an instance of the specified type.
        /// </summary>
        /// <param name="serializer">The serializer to deserialize with.</param>
        /// <param name="reader">The <see cref="JsonReader"/> containing the object.</param>
        /// <param name="objectType">The <see cref="Type"/> of object being deserialized.</param>
        /// <returns>The instance of objectType being deserialized.</returns>
        public static Task<object> DeserializeAsync(this JsonSerializer serializer, JsonReader reader, Type objectType)
            => Task.Factory.StartNew(() => serializer.Deserialize(reader, objectType));

        /// <summary>
        ///     Deserializes the JSON structure contained by the specified <see cref="JsonReader"/>
        ///     into an instance of the specified type..
        /// </summary>
        /// <param name="serializer">The serializer to deserialize with.</param>
        /// <param name="reader">
        ///     The <see cref="JsonReader"/> that contains the JSON structure to deserialize.
        /// </param>
        /// <param name="objectType">The <see cref="Type"/> of object being deserialized.</param>
        /// <returns>The instance of objectType being deserialized.</returns>
        public static Task<object> DeserializeAsync(this JsonSerializer serializer, TextReader reader, Type objectType)
            => Task.Factory.StartNew(() => serializer.Deserialize(reader, objectType));
    }
}
