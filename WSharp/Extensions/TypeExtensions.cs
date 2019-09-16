using System;

namespace WSharp.Extensions
{
    public static class TypeExtensions
    {
        public static object GetDefaultValue(this Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;
    }
}
