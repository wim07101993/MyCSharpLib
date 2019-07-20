using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyCSharpLib.Services.Serialization.Extensions
{
    public static class JsonSerializerExtensions
    {
        public static Task SerializeAsync(this Newtonsoft.Json.JsonSerializer serializer, TextWriter textWriter, object value)
            =>  Task.Factory.StartNew(() => serializer.Serialize(textWriter, value));

        public static Task SerializeAsync(this Newtonsoft.Json.JsonSerializer serializer, TextWriter textWriter, object value, Type objectType)
            => Task.Factory.StartNew(() => serializer.Serialize(textWriter, value, objectType));

        public static Task SerializeAsync(this Newtonsoft.Json.JsonSerializer serializer, JsonWriter jsonWriter, object value)
            => Task.Factory.StartNew(() => serializer.Serialize(jsonWriter, value));

        public static Task SerializeAsync(this Newtonsoft.Json.JsonSerializer serializer, JsonWriter jsonWriter, object value, Type objectType)
            => Task.Factory.StartNew(() => serializer.Serialize(jsonWriter, value, objectType));


        public static Task<object> DeserializeAsync(this Newtonsoft.Json.JsonSerializer serializer, JsonTextReader reader)
            => Task.Factory.StartNew(() => serializer.Deserialize(reader));

        public static Task<T> DeserializeAsync<T>(this Newtonsoft.Json.JsonSerializer serializer, JsonTextReader reader)
            => Task.Factory.StartNew(() => serializer.Deserialize<T>(reader));

        public static Task<object> DeserializeAsync(this Newtonsoft.Json.JsonSerializer serializer, JsonReader reader, Type objectType)
            => Task.Factory.StartNew(() => serializer.Deserialize(reader, objectType));

        public static Task<object> DeserializeAsync(this Newtonsoft.Json.JsonSerializer serializer, TextReader reader, Type objectType)
            => Task.Factory.StartNew(() => serializer.Deserialize(reader, objectType));

    }
}
