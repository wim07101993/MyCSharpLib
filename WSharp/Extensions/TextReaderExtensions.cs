using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSharp.Extensions
{
    public static class TextReaderExtensions
    {
        public static T DeserializeJson<T>(this TextReader reader)
            => new JsonSerializer().Deserialize<T>(new JsonTextReader(reader));

        public static object DeserializeJson(this TextReader reader, Type objectType)
            => new JsonSerializer().Deserialize(reader, objectType);

        public static async Task<T> DeserializeJsonAsync<T>(this TextReader reader)
            => await new JsonSerializer().DeserializeAsync<T>(new JsonTextReader(reader));

        public static Task<object> DeserializeJsonAsync(this TextReader reader, Type objectType)
            => new JsonSerializer().DeserializeAsync(reader, objectType);

        public static T DeserializeXml<T>(this TextReader reader)
            => new XmlSerializer(typeof(T)).Deserialize<T>(reader);

        public static object DeserializeXml(this TextReader reader, Type objectType)
            => new XmlSerializer(objectType).Deserialize(reader);

        public static async Task<T> DeserializeXmlAsync<T>(this TextReader reader)
            => await new XmlSerializer(typeof(T)).DeserializeAsync<T>(reader);

        public static async Task<object> DeserializeXmlAsync(this TextReader reader, Type objectType)
            => await new XmlSerializer(objectType).DeserializeAsync(reader);
    }
}