using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSharp.Extensions
{
    /// <summary>Extension methods for the <see cref="TextWriter"/> class.</summary>
    public static class TextWriterExtensions
    {
        /// <summary>Serializes a object to json through a <see cref="TextWriter"/>.</summary>
        /// <param name="writer">Writer to write the json to.</param>
        /// <param name="value">The value to serialize</param>
        public static void SerializeJson(this TextWriter writer, object value)
            => new JsonSerializer().Serialize(writer, value);

        /// <summary>Serializes a object to json through a <see cref="TextWriter"/>.</summary>
        /// <param name="writer">Writer to write the json to.</param>
        /// <param name="value">The value to serialize</param>
        public static async Task SerializeJsonAsync(this TextWriter writer, object value)
            => await new JsonSerializer().SerializeAsync(writer, value);

        /// <summary>Serializes a object to xml through a <see cref="TextWriter"/>.</summary>
        /// <param name="writer">Writer to write the json to.</param>
        /// <param name="value">The value to serialize</param>
        public static void SerializeXml(this TextWriter writer, object value)
            => new XmlSerializer(value.GetType()).Serialize(writer, value);

        /// <summary>Serializes a object to xml through a <see cref="TextWriter"/>.</summary>
        /// <param name="writer">Writer to write the json to.</param>
        /// <param name="value">The value to serialize</param>
        public static async Task SerializeXmlAsync(this TextWriter writer, object value)
            => await new XmlSerializer(value.GetType()).SerializeAsync(writer, value);
    }
}