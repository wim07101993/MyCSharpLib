using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSharp.Extensions
{
    public static class TextWriterExtensions
    {
        public static void SerializeJson(this TextWriter writer, object value)
            => new JsonSerializer().Serialize(writer, value);

        public static async Task SerializeJsonAsync(this TextWriter writer, object value)
            => await new JsonSerializer().SerializeAsync(writer, value);

        public static void SerializeXml(this TextWriter writer, object value)
            => new XmlSerializer(value.GetType()).Serialize(writer, value);

        public static async Task SerializeXmlAsync(this TextWriter writer, object value)
            => await new XmlSerializer(value.GetType()).SerializeAsync(writer, value);
    }
}