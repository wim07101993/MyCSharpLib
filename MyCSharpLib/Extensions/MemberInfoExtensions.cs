using MyCSharpLib.Reflection;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MyCSharpLib.Extensions
{
    public static class MemberInfoExtensions
    {
        public static string GetDisplayName(this MemberInfo memberInfo) 
            => memberInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? memberInfo.Name;

        public static string GetDescription(this MemberInfo memberInfo)
            => memberInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;

        public static bool IsBrowsable(this MemberInfo memberInfo)
            => memberInfo.GetCustomAttribute<BrowsableAttribute>()?.Browsable ?? true;

        public static object GetDefaultValue(this MemberInfo memberInfo)
        {
            var value = memberInfo.GetCustomAttribute<DefaultValueAttribute>()?.Value;
            if (value != null)
                return value;

            switch (memberInfo)
            {
                case PropertyInfo property:
                    return property.PropertyType.GetDefaultValue();
                case FieldInfo field:
                    return field.FieldType.GetDefaultValue();
                default:
                    return null;
            }
        }

        public static EditorBrowsableState GetEditableState(this MemberInfo memberInfo) 
            => memberInfo.GetCustomAttribute<EditorBrowsableAttribute>()?.State ?? EditorBrowsableState.Always;

        public static int GetAuthorizationLevels(this MemberInfo memberInfo)
            => memberInfo.GetCustomAttributes<AuthorizeAttribute>()
                ?.Select(x => x.AuthorizedLevels)
                .Aggregate(0, (acc, s) => acc | s) 
                ?? 0;
    }
}
