using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WSharp.Extensions;

namespace WSharp.Reflection
{
    public class SelectExpressionBuilder<T>
    {
        private List<object> _sections;


        public static Type Type { get; } = typeof(T);
        public string Path { get; set; }
        public string[] Delimitors { get; set; } = new[] { "/", ".", "\\" };

        public bool IncludeProperties { get; set; }
        public bool IncludeIndexProperties { get; set; }
        public bool IncludeEnumerableSearch { get; set; }
        public bool IncludeMethods { get; set; }

        public bool CaseSensitive { get; set; } = true;
        public bool AcceptDisplayName { get; set; } = true;


        public Expression<Func<T, object>> Build()
        {
            var splitPath = Path
                .Split(Delimitors, StringSplitOptions.RemoveEmptyEntries);
            var block = new ExpressionBuildBlock(Type, splitPath);

            return default;
        }

        public PropertyInfo GetProperty(string name)
            => IncludeProperties
            ? Type.GetProperties().FirstOrDefault(x => CompareName(x, name))
            : null;

        public MethodInfo GetMethod(string name)
            => IncludeMethods
            ? Type.GetMethods()
                .Where(x =>
                {
                    var parameters = x.GetParameters();
                    return parameters.Length == 0 || parameters.All(p => p.HasDefaultValue);
                })
                .FirstOrDefault(x => CompareName(x, name))
            : null;

        public PropertyInfo GetIndexProperty()
            => IncludeIndexProperties
            ? Type.GetProperties().FirstOrDefault(x => x.GetIndexParameters().Length > 0)
            : null;

        private bool CompareName(MemberInfo member, string name)
        {
            var comparisonType = CaseSensitive
                ? StringComparison.InvariantCulture
                : StringComparison.InvariantCultureIgnoreCase;

            var memberName = AcceptDisplayName
                ? member.GetDisplayName()
                : member.Name;

            return name.Equals(memberName, comparisonType);
        }
    }
}
