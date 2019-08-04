using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MyCSharpLib.Services.Serialization.Extensions
{
    public static class XmlSerializerExtensions
    {
        public static T Deserialize<T>(this System.Xml.Serialization.XmlSerializer serializer, Stream stream)
            => (T)serializer.Deserialize(stream);

        public static T Deserialize<T>(this System.Xml.Serialization.XmlSerializer serializer, TextReader reader)
            => (T)serializer.Deserialize(reader);

        public static T Deserialize<T>(this System.Xml.Serialization.XmlSerializer serializer, XmlReader reader)
            => (T)serializer.Deserialize(reader);

        public static T Deserialize<T>(this System.Xml.Serialization.XmlSerializer serializer, XmlReader reader, XmlDeserializationEvents events)
            => (T)serializer.Deserialize(reader, events);

        public static T Deserialize<T>(this System.Xml.Serialization.XmlSerializer serializer, XmlReader reader, string encodingStyle)
            => (T)serializer.Deserialize(reader, encodingStyle);


        #region async

        public static Task SerializeAsync(this System.Xml.Serialization.XmlSerializer serializer, Stream stream, object o)
            => Task.Factory.StartNew(() => serializer.Serialize(stream, o));

        public static Task SerializeAsync(this System.Xml.Serialization.XmlSerializer serializer, Stream stream, object o, XmlSerializerNamespaces namespaces)
            => Task.Factory.StartNew(() => serializer.Serialize(stream, o, namespaces));

        public static Task SerializeAsync(this System.Xml.Serialization.XmlSerializer serializer, TextWriter writer, object o)
            => Task.Factory.StartNew(() => serializer.Serialize(writer, o));

        public static Task SerializeAsync(this System.Xml.Serialization.XmlSerializer serializer, TextWriter textWriter, object o, XmlSerializerNamespaces namespaces)
            => Task.Factory.StartNew(() => serializer.Serialize(textWriter, o, namespaces));

        public static Task SerializeAsync(this System.Xml.Serialization.XmlSerializer serializer, XmlWriter xmlWriter, object o)
            => Task.Factory.StartNew(() => serializer.Serialize(xmlWriter, o));

        public static Task SerializeAsync(this System.Xml.Serialization.XmlSerializer serializer, XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces)
            => Task.Factory.StartNew(() => serializer.Serialize(xmlWriter, o, namespaces));

        public static Task SerializeAsync(this System.Xml.Serialization.XmlSerializer serializer, XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces, string encodingStyle)
            => Task.Factory.StartNew(() => serializer.Serialize(xmlWriter, o, namespaces, encodingStyle));

        public static Task SerializeAsync(this System.Xml.Serialization.XmlSerializer serializer, XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces, string encodingStyle, string id)
            => Task.Factory.StartNew(() => serializer.Serialize(xmlWriter, o, namespaces, encodingStyle, id));


        public static Task<T> DeserializeAsync<T>(this System.Xml.Serialization.XmlSerializer serializer, Stream stream)
            => Task.Factory.StartNew(() => serializer.Deserialize<T>(stream));

        public static Task<T> DeserializeAsync<T>(this System.Xml.Serialization.XmlSerializer serializer, TextReader textReader)
            => Task.Factory.StartNew(() => serializer.Deserialize<T>(textReader));

        public static Task<T> DeserializeAsync<T>(this System.Xml.Serialization.XmlSerializer serializer, XmlReader xmlReader)
            => Task.Factory.StartNew(() => serializer.Deserialize<T>(xmlReader));

        public static Task<T> DeserializeAsync<T>(this System.Xml.Serialization.XmlSerializer serializer, XmlReader xmlReader, XmlDeserializationEvents events)
            => Task.Factory.StartNew(() => serializer.Deserialize<T>(xmlReader, events));

        public static Task<T> DeserializeAsync<T>(this System.Xml.Serialization.XmlSerializer serializer, XmlReader xmlReader, string encodingStyle)
            => Task.Factory.StartNew(() => serializer.Deserialize<T>(xmlReader, encodingStyle));


        public static Task<object> DeserializeAsync(this System.Xml.Serialization.XmlSerializer serializer, Stream stream)
            => Task.Factory.StartNew(() => serializer.Deserialize(stream));

        public static Task<object> DeserializeAsync(this System.Xml.Serialization.XmlSerializer serializer, TextReader textReader)
            => Task.Factory.StartNew(() => serializer.Deserialize(textReader));

        public static Task<object> DeserializeAsync(this System.Xml.Serialization.XmlSerializer serializer, XmlReader xmlReader)
            => Task.Factory.StartNew(() => serializer.Deserialize(xmlReader));

        public static Task<object> DeserializeAsync(this System.Xml.Serialization.XmlSerializer serializer, XmlReader xmlReader, XmlDeserializationEvents events)
            => Task.Factory.StartNew(() => serializer.Deserialize(xmlReader, events));

        public static Task<object> DeserializeAsync(this System.Xml.Serialization.XmlSerializer serializer, XmlReader xmlReader, string encodingStyle)
            => Task.Factory.StartNew(() => serializer.Deserialize(xmlReader, encodingStyle));

        #endregion async
    }
}
