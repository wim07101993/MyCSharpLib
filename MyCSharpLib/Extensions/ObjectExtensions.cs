using System;

namespace MyCSharpLib.Extensions
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
    }
}
