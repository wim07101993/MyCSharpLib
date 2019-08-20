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
    }
}
