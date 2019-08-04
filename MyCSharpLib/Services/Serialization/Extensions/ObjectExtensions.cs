using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyCSharpLib.Services.Serialization.Extensions
{
    public static class ObjectExtensions
    {
        #region json async

        public static Task<string> SerializeJsonAsync(this object value)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value));

        public static Task<string> SerializeJsonAsync(this object value, Formatting formatting)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting));

        public static Task<string> SerializeJsonAsync(this object value, params JsonConverter[] converters)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, converters));

        public static Task<string> SerializeJsonAsync(this object value, Formatting formatting, params JsonConverter[] converters)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting, converters));

        public static Task<string> SerializeJsonAsync(this object value, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, settings));

        public static Task<string> SerializeJsonAsync(this object value, Type type, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, type, settings));

        public static Task<string> SerializeJsonAsync(this object value, Formatting formatting, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting, settings));

        public static Task<string> SerializeJsonAsync(this object value, Type type, Formatting formatting, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, type, formatting, settings));

        #endregion json async

        #region json

        public static string SerializeJson(this object value)
            => JsonConvert.SerializeObject(value);

        public static string SerializeJson(this object value, Formatting formatting)
            => JsonConvert.SerializeObject(value, formatting);

        public static string SerializeJson(this object value, params JsonConverter[] converters)
            => JsonConvert.SerializeObject(value, converters);

        public static string SerializeJson(this object value, Formatting formatting,
            params JsonConverter[] converters)
            => JsonConvert.SerializeObject(value, formatting, converters);

        public static string SerializeJson(this object value, JsonSerializerSettings settings)
            => JsonConvert.SerializeObject(value, settings);

        public static string SerializeJson(this object value, Type type,
            JsonSerializerSettings settings)
            => JsonConvert.SerializeObject(value, type, settings);

        public static string SerializeJson(this object value, Formatting formatting,
            JsonSerializerSettings settings)
            => JsonConvert.SerializeObject(value, formatting, settings);

        public static string SerializeJson(this object value, Type type, Formatting formatting,
            JsonSerializerSettings settings)
            => JsonConvert.SerializeObject(value, type, formatting, settings);

        #endregion json


        #region xml async

        public static async Task<string> SerializeXmlAsync(this object value)
        {
            using (var writer = new StringWriter())
            {
                await new System.Xml.Serialization.XmlSerializer(value.GetType()).SerializeAsync(writer, value);
                return writer.ToString();
            }
        }

        public static async Task<string> SerializeXmlAsync(this object value, XmlSerializerNamespaces namespaces)
        {
            using (var writer = new StringWriter())
            {
                await new System.Xml.Serialization.XmlSerializer(value.GetType()).SerializeAsync(writer, value, namespaces);
                return writer.ToString();
            }
        }

        #endregion xml async

        #region xml

        public static string SerializeXml(this object value)
        {
            using (var writer = new StringWriter())
            {
                new System.Xml.Serialization.XmlSerializer(value.GetType()).Serialize(writer, value);
                return writer.ToString();
            }
        }

        public static string SerializeXml(this object value, XmlSerializerNamespaces namespaces)
        {
            using (var writer = new StringWriter())
            {
                new System.Xml.Serialization.XmlSerializer(value.GetType()).Serialize(writer, value, namespaces);
                return writer.ToString();
            }
        }

        #endregion xml
    }
}
