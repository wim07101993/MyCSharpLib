using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WSharp.Extensions
{
    /// <summary>Extension methods for an <see cref="XmlSerializer"/>.</summary>
    public static class XmlSerializerExtensions
    {
        /// <summary>Deserializes the content of a <see cref="Stream"/> to an object using xml.</summary>
        /// <typeparam name="T">Type to deserialize the content to.</typeparam>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="stream">Stream to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(this XmlSerializer serializer, Stream stream)
            => (T)serializer.Deserialize(stream);

        /// <summary>Deserializes the content of a <see cref="TextReader"/> to an object using xml.</summary>
        /// <typeparam name="T">Type to deserialize the content to.</typeparam>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="reader">Reader to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(this XmlSerializer serializer, TextReader reader)
            => (T)serializer.Deserialize(reader);

        /// <summary>Deserializes the content of a <see cref="XmlReader"/> to an object using xml.</summary>
        /// <typeparam name="T">Type to deserialize the content to.</typeparam>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="reader">Reader to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(this XmlSerializer serializer, XmlReader reader)
            => (T)serializer.Deserialize(reader);

        /// <summary>Deserializes the content of a <see cref="TextReader"/> to an object using xml.</summary>
        /// <typeparam name="T">Type to deserialize the content to.</typeparam>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="reader">Reader to deserialize.</param>
        /// <param name="encodingStyle">Style of encoding</param>
        /// <returns>The deserialized object.</returns>
        public static T Deserialize<T>(this XmlSerializer serializer, XmlReader reader, string encodingStyle)
            => (T)serializer.Deserialize(reader, encodingStyle);

        #region async

        /// <summary>
        /// Serializes an object to a <see cref="Stream"/> using xml.
        /// </summary>
        /// <param name="serializer">Serializer to serialize the object</param>
        /// <param name="stream">Stream to serialize the object to.</param>
        /// <param name="o">The object to serialize.</param>
        public static Task SerializeAsync(this XmlSerializer serializer, Stream stream, object o)
            => Task.Factory.StartNew(() => serializer.Serialize(stream, o));

        /// <summary>
        /// Serializes an object to a <see cref="Stream"/> using xml.
        /// </summary>
        /// <param name="serializer">Serializer to serialize the object</param>
        /// <param name="stream">Stream to serialize the object to.</param>
        /// <param name="o">The object to serialize.</param>
        /// <param name="namespaces">The namespaces to use while serializing.</param>
        public static Task SerializeAsync(this XmlSerializer serializer, Stream stream, object o, XmlSerializerNamespaces namespaces)
            => Task.Factory.StartNew(() => serializer.Serialize(stream, o, namespaces));

        /// <summary>
        /// Serializes an object to a <see cref="TextWriter"/> using xml.
        /// </summary>
        /// <param name="serializer">Serializer to serialize the object</param>
        /// <param name="writer">Writer to serialize the object to.</param>
        /// <param name="o">The object to serialize.</param>
        public static Task SerializeAsync(this XmlSerializer serializer, TextWriter writer, object o)
            => Task.Factory.StartNew(() => serializer.Serialize(writer, o));

        /// <summary>
        /// Serializes an object to a <see cref="TextWriter"/> using xml.
        /// </summary>
        /// <param name="serializer">Serializer to serialize the object</param>
        /// <param name="textWriter">Writer to serialize the object to.</param>
        /// <param name="o">The object to serialize.</param>
        /// <param name="namespaces">The namespaces to use while serializing.</param>
        public static Task SerializeAsync(this XmlSerializer serializer, TextWriter textWriter, object o, XmlSerializerNamespaces namespaces)
            => Task.Factory.StartNew(() => serializer.Serialize(textWriter, o, namespaces));

        /// <summary>
        /// Serializes an object to a <see cref="XmlWriter"/> using xml.
        /// </summary>
        /// <param name="serializer">Serializer to serialize the object</param>
        /// <param name="xmlWriter">Writer to serialize the object to.</param>
        /// <param name="o">The object to serialize.</param>
        public static Task SerializeAsync(this XmlSerializer serializer, XmlWriter xmlWriter, object o)
            => Task.Factory.StartNew(() => serializer.Serialize(xmlWriter, o));

        /// <summary>
        /// Serializes an object to a <see cref="XmlWriter"/> using xml.
        /// </summary>
        /// <param name="serializer">Serializer to serialize the object</param>
        /// <param name="xmlWriter">Writer to serialize the object to.</param>
        /// <param name="o">The object to serialize.</param>
        /// <param name="namespaces">The namespaces to use while serializing.</param>
        public static Task SerializeAsync(this XmlSerializer serializer, XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces)
            => Task.Factory.StartNew(() => serializer.Serialize(xmlWriter, o, namespaces));

        /// <summary>
        /// Serializes an object to a <see cref="XmlWriter"/> using xml.
        /// </summary>
        /// <param name="serializer">Serializer to serialize the object</param>
        /// <param name="xmlWriter">Writer to serialize the object to.</param>
        /// <param name="o">The object to serialize.</param>
        /// <param name="namespaces">The namespaces to use while serializing.</param>
        /// <param name="encodingStyle">The encoding style to serialize the object.</param>
        public static Task SerializeAsync(this XmlSerializer serializer, XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces, string encodingStyle)
            => Task.Factory.StartNew(() => serializer.Serialize(xmlWriter, o, namespaces, encodingStyle));

        /// <summary>Deserializes the content of a <see cref="Stream"/> to an object using xml.</summary>
        /// <typeparam name="T">Type to deserialize the content to.</typeparam>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="stream">Stream to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static Task<T> DeserializeAsync<T>(this XmlSerializer serializer, Stream stream)
            => Task.Factory.StartNew(() => serializer.Deserialize<T>(stream));

        /// <summary>Deserializes the content of a <see cref="TextReader"/> to an object using xml.</summary>
        /// <typeparam name="T">Type to deserialize the content to.</typeparam>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="textReader">Reader to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static Task<T> DeserializeAsync<T>(this XmlSerializer serializer, TextReader textReader)
            => Task.Factory.StartNew(() => serializer.Deserialize<T>(textReader));

        /// <summary>Deserializes the content of a <see cref="XmlReader"/> to an object using xml.</summary>
        /// <typeparam name="T">Type to deserialize the content to.</typeparam>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="reader">Reader to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static Task<T> DeserializeAsync<T>(this XmlSerializer serializer, XmlReader xmlReader)
            => Task.Factory.StartNew(() => serializer.Deserialize<T>(xmlReader));

        /// <summary>Deserializes the content of a <see cref="TextReader"/> to an object using xml.</summary>
        /// <typeparam name="T">Type to deserialize the content to.</typeparam>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="reader">Reader to deserialize.</param>
        /// <param name="encodingStyle">Style of encoding</param>
        /// <returns>The deserialized object.</returns>
        public static Task<T> DeserializeAsync<T>(this XmlSerializer serializer, XmlReader xmlReader, string encodingStyle)
            => Task.Factory.StartNew(() => serializer.Deserialize<T>(xmlReader, encodingStyle));

        /// <summary>Deserializes the content of a <see cref="Stream"/> to an object using xml.</summary>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="stream">Stream to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static Task<object> DeserializeAsync(this XmlSerializer serializer, Stream stream)
            => Task.Factory.StartNew(() => serializer.Deserialize(stream));

        /// <summary>Deserializes the content of a <see cref="TextReader"/> to an object using xml.</summary>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="textReader">Reader to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static Task<object> DeserializeAsync(this XmlSerializer serializer, TextReader textReader)
            => Task.Factory.StartNew(() => serializer.Deserialize(textReader));

        /// <summary>Deserializes the content of a <see cref="XmlReader"/> to an object using xml.</summary>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="reader">Reader to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static Task<object> DeserializeAsync(this XmlSerializer serializer, XmlReader xmlReader)
            => Task.Factory.StartNew(() => serializer.Deserialize(xmlReader));

        /// <summary>Deserializes the content of a <see cref="TextReader"/> to an object using xml.</summary>
        /// <param name="serializer">Serializer to deserialize the content of the stream.</param>
        /// <param name="reader">Reader to deserialize.</param>
        /// <param name="encodingStyle">Style of encoding</param>
        /// <returns>The deserialized object.</returns>
        public static Task<object> DeserializeAsync(this XmlSerializer serializer, XmlReader xmlReader, string encodingStyle)
            => Task.Factory.StartNew(() => serializer.Deserialize(xmlReader, encodingStyle));

        #endregion async
    }
}