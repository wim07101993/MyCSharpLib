using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSharp.Extensions
{
    public static class ObjectExtensions
    {
        public static object Cast(this object obj, Type type)
        {
            if (obj == null || obj.GetType() == type)
                return obj;
            return Convert.ChangeType(obj, type);
        }

        public static T CastObject<T>(this object obj)
        {
            if (obj == null)
                return default;

            if (obj.GetType() == typeof(T))
                return (T)obj;

            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public static bool Validate(this object obj, out List<ValidationException> validationErrors)
        {
            var properties = obj.GetType().GetProperties();

            validationErrors = new List<ValidationException>();

            foreach (var property in properties)
            {
                if (property.GetIndexParameters().Any())
                    continue;

                var value = property.GetValue(obj);
                var attributes = property.GetCustomAttributes<ValidationAttribute>();
                foreach (var attribute in attributes)
                {
                    var result = attribute.GetValidationResult(value, new ValidationContext(obj));
                    if (result != ValidationResult.Success)
                        validationErrors.Add(new ValidationException(result, attribute, value));
                }

                if (value == null)
                    continue;

                value.Validate(property, ref validationErrors);
                if (value is IEnumerable enumerable)
                {
                    foreach (var e in enumerable)
                        e.Validate(property, ref validationErrors);
                }
            }

            return !validationErrors.Any();
        }

        private static bool Validate(this object obj, PropertyInfo parentProperty, ref List<ValidationException> validationErrors)
        {
            if (validationErrors == null)
                validationErrors = new List<ValidationException>();

            var properties = obj.GetType()
                .GetProperties()
                .Where(x => x.CanRead &&
                    // no index property
                    !x.GetIndexParameters().Any() &&
                    // not static
                    !x.GetGetMethod().IsStatic &&
                    // is not a child property of itself
                    !(x.PropertyType == parentProperty.PropertyType &&
                    x.PropertyType == parentProperty.DeclaringType &&
                    x.DeclaringType == parentProperty.DeclaringType))
                .Select(x => new
                {
                    Info = x,
                    Value = x.GetValue(obj),
                    Attributes = x.GetCustomAttributes<ValidationAttribute>()
                });

            foreach (var property in properties)
            {
                foreach (var attribute in property.Attributes)
                {
                    var result = attribute.GetValidationResult(property.Value, new ValidationContext(obj));
                    if (result != ValidationResult.Success)
                        validationErrors.Add(new ValidationException(result, attribute, property.Value));
                }

                if (property.Value == null)
                    continue;

                property.Value.Validate(property.Info, ref validationErrors);
                if (property.Value is IEnumerable enumerable)
                {
                    foreach (var e in enumerable)
                        e.Validate(property.Info, ref validationErrors);
                }
            }

            return !validationErrors.Any();
        }

        #region json async

        public static Task<string> SerializeJsonAsync(this object value, Formatting formatting = Formatting.Indented)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting));

        public static Task<string> SerializeJsonAsync(this object value, Formatting formatting = Formatting.Indented, params JsonConverter[] converters)
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

        public static string SerializeJson(this object value, Formatting formatting = Formatting.Indented)
            => JsonConvert.SerializeObject(value, formatting);

        public static string SerializeJson(this object value, Formatting formatting = Formatting.Indented, params JsonConverter[] converters)
            => JsonConvert.SerializeObject(value, formatting, converters);

        public static string SerializeJson(this object value, JsonSerializerSettings settings)
            => JsonConvert.SerializeObject(value, settings);

        public static string SerializeJson(this object value, Type type, JsonSerializerSettings settings)
            => JsonConvert.SerializeObject(value, type, settings);

        public static string SerializeJson(this object value, JsonSerializerSettings settings, Formatting formatting = Formatting.Indented)
            => JsonConvert.SerializeObject(value, formatting, settings);

        public static string SerializeJson(this object value, Type type, JsonSerializerSettings settings, Formatting formatting = Formatting.Indented)
            => JsonConvert.SerializeObject(value, type, formatting, settings);

        #endregion json

        #region xml async

        public static async Task<string> SerializeXmlAsync(this object value)
        {
            using (var writer = new StringWriter())
            {
                await new XmlSerializer(value.GetType()).SerializeAsync(writer, value);
                return writer.ToString();
            }
        }

        public static async Task<string> SerializeXmlAsync(this object value, XmlSerializerNamespaces namespaces)
        {
            using (var writer = new StringWriter())
            {
                await new XmlSerializer(value.GetType()).SerializeAsync(writer, value, namespaces);
                return writer.ToString();
            }
        }

        #endregion xml async

        #region xml

        public static string SerializeXml(this object value)
        {
            using (var writer = new StringWriter())
            {
                new XmlSerializer(value.GetType()).Serialize(writer, value);
                return writer.ToString();
            }
        }

        public static string SerializeXml(this object value, XmlSerializerNamespaces namespaces)
        {
            using (var writer = new StringWriter())
            {
                new XmlSerializer(value.GetType()).Serialize(writer, value, namespaces);
                return writer.ToString();
            }
        }

        #endregion xml
    }
}