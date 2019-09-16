using System.IO;
using System.Threading.Tasks;

namespace WSharp.Services.Serialization.Extensions
{
    public static class TextWriterExtensions
    {
        public static void SerializeJson(this TextWriter writer, object value)
            => new Newtonsoft.Json.JsonSerializer().Serialize(writer, value);

        public static async Task SerializeJsonAsync(this TextWriter writer, object value)
            => await new Newtonsoft.Json.JsonSerializer().SerializeAsync(writer, value);

        public static void SerializeXml(this TextWriter writer, object value)
            => new System.Xml.Serialization.XmlSerializer(value.GetType()).Serialize(writer, value);

        public static async Task SerializeXmlAsync(this TextWriter writer, object value)
            => await new System.Xml.Serialization.XmlSerializer(value.GetType()).SerializeAsync(writer, value);
    }
}
