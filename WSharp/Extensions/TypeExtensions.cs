using System;

namespace WSharp.Extensions
{
    /// <summary>Extension methods for a type.</summary>
    public static class TypeExtensions
    {
        /// <summary>Gets the default value of this type.</summary>
        /// <param name="type">Type to get the default value of.</param>
        /// <returns>The default value of this type.</returns>
        public static object GetDefaultValue(this Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}