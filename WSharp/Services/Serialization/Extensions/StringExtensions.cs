using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WSharp.Services.Serialization.Extensions
{
    public static class StringExtensions
    {
        #region json async

        public static Task<object> DeserializeJsonAsync(this string value)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value));

        public static Task<object> DeserializeJsonAsync(this string value, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, settings));

        public static Task<object> DeserializeJsonAsync(this string value, Type type)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, type));

        public static Task<object> DeserializeJsonObjectAsync(this string value, Type type,
            params JsonConverter[] converters)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, type, converters));

        public static Task<object> DeserializeJsonObjectAsync(this string value, Type type,
            JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, type, settings));

        public static Task<T> DeserializeJsonAsync<T>(this string value)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value));

        public static Task<T> DeserializeAnonymousJsonTypeAsync<T>(this string value, T anonymousTypeObject)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject));

        public static Task<T> DeserializeAnonymousJsonTypeAsync<T>(this string value, T anonymousTypeObject, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject, settings));

        public static Task<T> DeserializeJsonObjectAsync<T>(string value, params JsonConverter[] converters)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value, converters));

        public static Task<T> DeserializeJsonAsync<T>(this string value, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value, settings));

        #endregion json async

        #region json

        public static object DeserializeJson(this string value)
            => JsonConvert.DeserializeObject(value);

        public static object DeserializeJson(this string value, JsonSerializerSettings settings)
            => JsonConvert.DeserializeObject(value, settings);

        public static object DeserializeJson(this string value, Type type)
            => JsonConvert.DeserializeObject(value, type);

        public static object DeserializeJsonObject(this string value, Type type, params JsonConverter[] converters)
            => JsonConvert.DeserializeObject(value, type, converters);

        public static object DeserializeJsonObject(this string value, Type type, JsonSerializerSettings settings)
            => JsonConvert.DeserializeObject(value, type, settings);

        public static T DeserializeJson<T>(this string value)
            => JsonConvert.DeserializeObject<T>(value);

        public static T DeserializeAnonymousJsonType<T>(this string value, T anonymousTypeObject)
            => JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject);

        public static T DeserializeAnonymousJsonType<T>(this string value, T anonymousTypeObject, JsonSerializerSettings settings)
            => JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject, settings);

        public static T DeserializeJsonObject<T>(string value, params JsonConverter[] converters)
            => JsonConvert.DeserializeObject<T>(value, converters);

        public static T DeserializeJson<T>(this string value, JsonSerializerSettings settings)
            => JsonConvert.DeserializeObject<T>(value, settings);

        #endregion json


        #region xml async

        public static async Task<T> DeserializeXmlAsync<T>(this string s)
        {
            using (var reader = new StringReader(s))
                return await reader.DeserializeXmlAsync<T>();
        }

        #endregion xml async


        #region xml

        public static T DeserializeXml<T>(this string s)
        {
            using (var reader = new StringReader(s))
                return reader.DeserializeXml<T>();
        }

        #endregion xml
    }
}
