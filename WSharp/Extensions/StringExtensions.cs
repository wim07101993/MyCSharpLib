using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace WSharp.Extensions
{
    /// <summary>Extension methods for <see cref="string"/> s.</summary>
    public static class StringExtensions
    {
        /// <summary>Converts a string to the ASCII equivalent in bytes</summary>
        /// <param name="str"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="EncoderFallbackException">
        ///     A fallback occurred (see ~/docs/standard/base-types/character-encoding.md for
        ///     complete explanation) -and- <see cref="Encoding.EncoderFallback"/> is set to <see cref="EncoderExceptionFallback"/>.
        /// </exception>
        /// <returns>The ASCII equivalent of the string input</returns>
        public static byte[] ToAsciiBytes(this string str) => Encoding.ASCII.GetBytes(str);

        /// <summary>Returns the string value./&gt;</summary>
        /// <param name="str">The <see cref="SecureString"/> to get the string value from.</param>
        /// <returns>The string value of the password.</returns>
        public static string GetValue(this SecureString str)
        {
            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(str);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        #region json async

        /// <summary>Deserializes a json string to an object.</summary>
        /// <param name="value">The serialized value.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to deserialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized value.</returns>
        public static Task<object> DeserializeJsonAsync(this string value, JsonSerializerSettings settings = default)
        {
            if (settings == default)
                return Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value));
            else
                return Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, settings));
        }

        /// <summary>Deserializes a json string to an object.</summary>
        /// <param name="value">The serialized value.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <returns>The deserialized value.</returns>
        public static Task<object> DeserializeJsonAsync(this string value, Type type)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, type));

        /// <summary>Deserializes a json string to an object.</summary>
        /// <param name="value">The serialized value.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized value.</returns>
        public static Task<object> DeserializeJsonObjectAsync(this string value, Type type, params JsonConverter[] converters)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, type, converters));

        /// <summary>Deserializes a json string to an object.</summary>
        /// <param name="value">The serialized value.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to deserialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized value.</returns>
        public static Task<object> DeserializeJsonObjectAsync(this string value, Type type, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, type, settings));

        /// <summary>Deserializes a json string to an object.</summary>
        /// <param name="value">The serialized value.</param>
        /// <returns>The deserialized value.</returns>
        public static Task<T> DeserializeJsonAsync<T>(this string value)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value));

        /// <summary>Deserializes a json string to an object.</summary>
        /// <typeparam name="T">Type to deserialize the string to.</typeparam>
        /// <param name="value">The serialized value.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <returns>The deserialized value.</returns>
        public static Task<T> DeserializeAnonymousJsonTypeAsync<T>(this string value, T anonymousTypeObject)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject));

        /// <summary>Deserializes a json string to an object.</summary>
        /// <typeparam name="T">Type to deserialize the string to.</typeparam>
        /// <param name="value">The serialized value.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to deserialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized value.</returns>
        public static Task<T> DeserializeAnonymousJsonTypeAsync<T>(this string value, T anonymousTypeObject, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject, settings));

        /// <summary>Deserializes a json string to an object.</summary>
        /// <typeparam name="T">Type to deserialize the string to.</typeparam>
        /// <param name="value">The serialized value.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized value.</returns>
        public static Task<T> DeserializeJsonObjectAsync<T>(string value, params JsonConverter[] converters)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value, converters));

        /// <summary>Deserializes a json string to an object.</summary>
        /// <typeparam name="T">Type to deserialize the string to.</typeparam>
        /// <param name="value">The serialized value.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to deserialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized value.</returns>
        public static Task<T> DeserializeJsonAsync<T>(this string value, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value, settings));

        #endregion json async

        #region json

        /// <summary>Deserializes a json string to an object.</summary>
        /// <param name="value">The serialized value.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to deserialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized value.</returns>
        public static object DeserializeJson(this string value, JsonSerializerSettings settings = default)
        {
            if (settings == default)
                return Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value));
            else
                return Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, settings));
        }

        /// <summary>Deserializes a json string to an object.</summary>
        /// <param name="value">The serialized value.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <returns>The deserialized value.</returns>
        public static object DeserializeJson(this string value, Type type)
            => JsonConvert.DeserializeObject(value, type);

        /// <summary>Deserializes a json string to an object.</summary>
        /// <param name="value">The serialized value.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized value.</returns>
        public static object DeserializeJsonObject(this string value, Type type, params JsonConverter[] converters)
            => JsonConvert.DeserializeObject(value, type, converters);

        /// <summary>Deserializes a json string to an object.</summary>
        /// <param name="value">The serialized value.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to deserialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized value.</returns>
        public static object DeserializeJsonObject(this string value, Type type, JsonSerializerSettings settings)
            => JsonConvert.DeserializeObject(value, type, settings);

        /// <summary>Deserializes a json string to an object.</summary>
        /// <param name="value">The serialized value.</param>
        /// <returns>The deserialized value.</returns>
        public static T DeserializeJson<T>(this string value)
            => JsonConvert.DeserializeObject<T>(value);

        /// <summary>Deserializes a json string to an object.</summary>
        /// <typeparam name="T">Type to deserialize the string to.</typeparam>
        /// <param name="value">The serialized value.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <returns>The deserialized value.</returns>
        public static T DeserializeAnonymousJsonType<T>(this string value, T anonymousTypeObject)
            => JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject);

        /// <summary>Deserializes a json string to an object.</summary>
        /// <typeparam name="T">Type to deserialize the string to.</typeparam>
        /// <param name="value">The serialized value.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to deserialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized value.</returns>
        public static T DeserializeAnonymousJsonType<T>(this string value, T anonymousTypeObject, JsonSerializerSettings settings)
            => JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject, settings);

        /// <summary>Deserializes a json string to an object.</summary>
        /// <typeparam name="T">Type to deserialize the string to.</typeparam>
        /// <param name="value">The serialized value.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized value.</returns>
        public static T DeserializeJsonObject<T>(string value, params JsonConverter[] converters)
            => JsonConvert.DeserializeObject<T>(value, converters);

        /// <summary>Deserializes a json string to an object.</summary>
        /// <typeparam name="T">Type to deserialize the string to.</typeparam>
        /// <param name="value">The serialized value.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to deserialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized value.</returns>
        public static T DeserializeJson<T>(this string value, JsonSerializerSettings settings)
            => JsonConvert.DeserializeObject<T>(value, settings);

        #endregion json

        /// <summary>Deserializes a xnml string to an object.</summary>
        /// <typeparam name="T">The type to deserialize the string to.</typeparam>
        /// <param name="s">The string to deserialized.</param>
        /// <returns>The deserialized object.</returns>
        public static async Task<T> DeserializeXmlAsync<T>(this string s)
        {
            using var reader = new StringReader(s);

            return await reader.DeserializeXmlAsync<T>();
        }

        /// <summary>Deserializes a xnml string to an object.</summary>
        /// <typeparam name="T">The type to deserialize the string to.</typeparam>
        /// <param name="s">The string to deserialized.</param>
        /// <returns>The deserialized object.</returns>
        public static T DeserializeXml<T>(this string s)
        {
            using var reader = new StringReader(s);

            return reader.DeserializeXml<T>();
        }
    }
}
