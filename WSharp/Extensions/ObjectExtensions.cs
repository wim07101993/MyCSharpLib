using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

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
        
        public static bool Validate(this object obj, 
            out List<ValidationException> validationErrors)
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

                if (!value.Validate(out var subErrors))
                    validationErrors.AddRange(subErrors);

                if (value is IEnumerable enumerable)
                {
                    foreach (var e in enumerable)
                        if (!e.Validate(out var enumerableErrors))
                            validationErrors.AddRange(enumerableErrors);
                }
            }

            return !validationErrors.Any();
        }
    }
}
