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
using WSharp.Reflection;

namespace WSharp.Extensions
{
    /// <summary>Extension methods for <see cref="object"/> s.</summary>
    public static class ObjectExtensions
    {
        /// <summary>Cast an object to another type.</summary>
        /// <param name="obj">The object to cast.</param>
        /// <param name="type">The type to cast the object to.</param>
        /// <returns>The casted object.</returns>
        public static object CastObject(this object obj, Type type)
        {
            return obj == null || obj.GetType() == type
                ? obj
                : Convert.ChangeType(obj, type);
        }

        /// <summary>Cast an object to another type.</summary>
        /// <typeparam name="T">he type to cast the object to.</typeparam>
        /// <param name="obj">The object to cast.</param>
        /// <returns>The casted object.</returns>
        public static T CastObject<T>(this object obj)
        {
            if (obj == null)
                return default;

            if (obj.GetType() == typeof(T))
                return (T)obj;

            return (T)Convert.ChangeType(obj, typeof(T));
        }

        /// <summary>
        ///     Validates an object using the <see cref="ValidationAttribute"/> s declared on the
        ///     different properties of the object. (works recursivly)
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="validationErrors">The errors when the validation was unsuccessful.</param>
        /// <returns>Whether the object was validated successfully.</returns>
        public static bool Validate(this object obj, out List<ValidationException> validationErrors) 
            => obj.Validate(false, out validationErrors);

        public static bool Validate(this object obj, bool forceExtensionMethod, out List<ValidationException> validationErrors)
        {
            if (!forceExtensionMethod && obj is IValidatable validatable)
            {
                var success = validatable.TryValidate(out var exception);
                validationErrors = exception.InnerExceptions
                    .Where(x => x is ValidationException)
                    .Cast<ValidationException>()
                    .ToList();
                return success;
            }

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

        /// <summary>
        ///     Validates an object using the <see cref="ValidationAttribute"/> s declared on the
        ///     different properties of the object. (works recursivly)
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="parentProperty">The property of which this object was the value.</param>
        /// <param name="validationErrors">The errors when the validation was unsuccessful.</param>
        /// <returns>Whether the object was validated successfully.</returns>
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
                    Attributes = x.GetCustomAttributes<ValidationAttribute>()
                })
                .Where(x => x.Attributes.Any())
                .Select(x =>
                {
                    object value;
                    try
                    {
                        value = x.Info.GetValue(obj);
                    }
                    catch (Exception)
                    {
                        value = Invalid.Value;
                    }
                    return new
                    {
                        x.Info,
                        x.Attributes,
                        Value = value
                    };
                });

            foreach (var property in properties)
            {
                if (property.Value == Invalid.Value)
                    validationErrors.Add(new ValidationException("Could not get the value of the property"));

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

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>The serialized object.</returns>
        public static Task<string> SerializeJsonAsync(this object value, Formatting formatting = Formatting.Indented)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting));

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        /// <returns>The serialized object.</returns>
        public static Task<string> SerializeJsonAsync(this object value, Formatting formatting = Formatting.Indented, params JsonConverter[] converters)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting, converters));

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to serialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The serialized object.</returns>
        public static Task<string> SerializeJsonAsync(this object value, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, settings));

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="type">
        ///     The type of the value being serialized. This parameter is used when
        ///     <see cref="TypeNameHandling"/> is <see cref="TypeNameHandling.Auto"/> to write out
        ///     the type name if the type of the value does not match. Specifying the type is optional.
        /// </param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to serialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The serialized object.</returns>
        public static Task<string> SerializeJsonAsync(this object value, Type type, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, type, settings));

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to serialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The serialized object.</returns>
        public static Task<string> SerializeJsonAsync(this object value, Formatting formatting, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting, settings));

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="type">
        ///     The type of the value being serialized. This parameter is used when
        ///     <see cref="TypeNameHandling"/> is <see cref="TypeNameHandling.Auto"/> to write out
        ///     the type name if the type of the value does not match. Specifying the type is optional.
        /// </param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to serialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The serialized object.</returns>
        public static Task<string> SerializeJsonAsync(this object value, Type type, Formatting formatting, JsonSerializerSettings settings)
            => Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, type, formatting, settings));

        #endregion json async

        #region json

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>The serialized object.</returns>
        public static string SerializeJson(this object value, Formatting formatting = Formatting.Indented)
            => JsonConvert.SerializeObject(value, formatting);

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        /// <returns>The serialized object.</returns>
        public static string SerializeJson(this object value, Formatting formatting = Formatting.Indented, params JsonConverter[] converters)
            => JsonConvert.SerializeObject(value, formatting, converters);

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to serialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The serialized object.</returns>
        public static string SerializeJson(this object value, JsonSerializerSettings settings)
            => JsonConvert.SerializeObject(value, settings);

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="type">
        ///     The type of the value being serialized. This parameter is used when
        ///     <see cref="TypeNameHandling"/> is <see cref="TypeNameHandling.Auto"/> to write out
        ///     the type name if the type of the value does not match. Specifying the type is optional.
        /// </param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to serialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The serialized object.</returns>
        public static string SerializeJson(this object value, Type type, JsonSerializerSettings settings)
            => JsonConvert.SerializeObject(value, type, settings);

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to serialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The serialized object.</returns>
        public static string SerializeJson(this object value, JsonSerializerSettings settings, Formatting formatting = Formatting.Indented)
            => JsonConvert.SerializeObject(value, formatting, settings);

        /// <summary>Serializes an object using json.</summary>
        /// <param name="value">The object to serialized.</param>
        /// <param name="type">
        ///     The type of the value being serialized. This parameter is used when
        ///     <see cref="TypeNameHandling"/> is <see cref="TypeNameHandling.Auto"/> to write out
        ///     the type name if the type of the value does not match. Specifying the type is optional.
        /// </param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">
        ///     The <see cref="JsonSerializerSettings"/> used to serialize the object. If this is
        ///     null, default serialization settings will be used.
        /// </param>
        /// <returns>The serialized object.</returns>
        public static string SerializeJson(this object value, Type type, JsonSerializerSettings settings, Formatting formatting = Formatting.Indented)
            => JsonConvert.SerializeObject(value, type, formatting, settings);

        #endregion json

        /// <summary>Serializes an object using xml.</summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="namespaces">The namespaces to use while serailizing.</param>
        /// <returns>The serialized value.</returns>
        public static async Task<string> SerializeXmlAsync(this object value, XmlSerializerNamespaces namespaces = default)
        {
            using (var writer = new StringWriter())
            {
                if (namespaces == default)
                    await new XmlSerializer(value.GetType()).SerializeAsync(writer, value);
                else
                    await new XmlSerializer(value.GetType()).SerializeAsync(writer, value, namespaces);

                return writer.ToString();
            }
        }

        /// <summary>Serializes an object using xml.</summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="namespaces">The namespaces to use while serailizing.</param>
        /// <returns>The serialized value.</returns>
        public static string SerializeXml(this object value, XmlSerializerNamespaces namespaces = default)
        {
            using (var writer = new StringWriter())
            {
                if (namespaces == default)
                    new XmlSerializer(value.GetType()).Serialize(writer, value);
                else
                    new XmlSerializer(value.GetType()).Serialize(writer, value, namespaces);

                return writer.ToString();
            }
        }
    }

    internal class Invalid 
    {
        public static Invalid Value { get; } = new Invalid();
    }
}