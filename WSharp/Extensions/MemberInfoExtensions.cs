using System.ComponentModel;
using System.Linq;
using System.Reflection;
using WSharp.Reflection;

namespace WSharp.Extensions
{
    /// <summary>Extension methods for the <see cref="MemberInfo"/> class.</summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        ///     Returns the <see cref="DisplayNameAttribute.DisplayName"/> of a
        ///     <see cref="MemberInfo"/>. If the <see cref="DisplayNameAttribute"/> is not declared,
        ///     the <see cref="MemberInfo.Name"/> property is used.
        /// </summary>
        /// <param name="memberInfo">The member to get the name of.</param>
        /// <returns>
        ///     The <see cref="DisplayNameAttribute.DisplayName"/> or <see cref="MemberInfo.Name"/>
        ///     of the member.
        /// </returns>
        public static string GetDisplayName(this MemberInfo memberInfo)
            => memberInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? memberInfo.Name;

        /// <summary>Returns the <see cref="DescriptionAttribute.Description"/> of a <see cref="MemberInfo"/>.</summary>
        /// <param name="memberInfo">The member to get the description of.</param>
        /// <returns>The <see cref="DescriptionAttribute.Description"/> of the member.</returns>
        public static string GetDescription(this MemberInfo memberInfo)
            => memberInfo.GetCustomAttribute<DescriptionAttribute>()?.Description;

        /// <summary>Returns whether the <see cref="MemberInfo"/> is browsable ( <see cref="BrowsableAttribute.Browsable"/>.</summary>
        /// <param name="memberInfo">The member to get the browsable state of.</param>
        /// <returns>Whether the <see cref="MemberInfo"/> is browsable ( <see cref="BrowsableAttribute.Browsable"/>.</returns>
        public static bool IsBrowsable(this MemberInfo memberInfo)
            => memberInfo.GetCustomAttribute<BrowsableAttribute>()?.Browsable ?? true;

        /// <summary>
        ///     Returns the default value of a property using the
        ///     <see cref="DefaultValueAttribute"/>. If this attribute is not declared, the default
        ///     value of the type of the property is used.
        /// </summary>
        /// <param name="memberInfo">The member to get the default value of.</param>
        /// <returns>The default value of a member.</returns>
        public static object GetDefaultValue(this PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<DefaultValueAttribute>();
            return attribute == null
                ? property.PropertyType.GetDefaultValue()
                : attribute.Value;
        }

        /// <summary>
        ///     Returns the default value of a field using the <see cref="DefaultValueAttribute"/>.
        ///     If this attribute is not declared, the default value of the type of the property is used.
        /// </summary>
        /// <param name="memberInfo">The member to get the name of.</param>
        /// <returns>The default value of a member.</returns>
        public static object GetDefaultValue(this FieldInfo field)
        {
            var attribute = field.GetCustomAttribute<DefaultValueAttribute>();
            return attribute == null
                ? field.FieldType.GetDefaultValue()
                : attribute.Value;
        }

        /// <summary>
        ///     Returns whether the given member can be browsable in the editor using the
        ///     <see cref="EditorBrowsableAttribute"/>. (If the attribute is not declared,
        ///     <see cref="EditorBrowsableState.Always"/> is returned).
        /// </summary>
        /// <param name="memberInfo">The member to get the editor browsable state of.</param>
        /// <returns>Whether the given member can be browsable in the editor using the <see cref="EditorBrowsableAttribute"/>.</returns>
        public static EditorBrowsableState GetEditableState(this MemberInfo memberInfo)
            => memberInfo.GetCustomAttribute<EditorBrowsableAttribute>()?.State ?? EditorBrowsableState.Always;

        /// <summary>
        ///     Returns the authorization levels that are attached to a member using the
        ///     <see cref="AuthorizeAttribute"/>. (0 if there are none)
        /// </summary>
        /// <param name="memberInfo">The member to get the authorize levels of.</param>
        /// <returns>The authorization levels that are attached to a member.</returns>
        public static int GetAuthorizationLevels(this MemberInfo memberInfo)
            => memberInfo.GetCustomAttributes<AuthorizeAttribute>()
                ?.Select(x => x.AuthorizedLevels)
                .Aggregate(0, (acc, s) => acc | s)
                ?? 0;
    }
}