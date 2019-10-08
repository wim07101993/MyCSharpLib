using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WSharp.Extensions;

namespace WSharp.Reflection
{
    public class ExpressionBuildBlock
    {
        public ExpressionBuildBlock(Type parentType, IEnumerable<string> sections)
        {
            ParentType = parentType;
            Sections = sections.ToList();
            BuildNextBlock();
        }


        public List<string> Sections { get; set; }

        public string Section => Sections.First();
        public Type ParentType { get; }
        public ExpressionBuildBlock NextBlock { get; private set; }

        public bool IncludeProperties { get; set; } = true;
        public bool IncludeIndexProperties { get; set; } = true;
        public bool IncludeEnumerableSearch { get; set; } = true;
        public bool IncludeMethods { get; set; } = true;

        public bool CaseSensitive { get; set; } = true;
        public bool AcceptDisplayName { get; set; } = true;

        public PropertyInfo Property { get; set; }
        public MethodInfo Method { get; set; }
        public PropertyInfo IndexProperty { get; set; }
        public ParameterInfo Parameter { get; set; }
        public object ParameterValue { get; set; }


        private void BuildNextBlock()
        {
            var section = Section;
            Property = GetProperty(section);
            if (Property != null)
            {
                NextBlock = new ExpressionBuildBlock(Property.PropertyType, Sections.Skip(1));
                return;
            }

            Method = GetMethod(section);
            if (Method != null)
            {
                NextBlock = new ExpressionBuildBlock(Method.ReturnType, Sections.Skip(1));
                return;
            }

            IndexProperty = GetIndexProperty();
            if (IndexProperty != null)
            {
                Parameter = IndexProperty.GetIndexParameters().First();
                try
                {
                    ParameterValue = Convert.ChangeType(section, Parameter.ParameterType);
                    NextBlock = new ExpressionBuildBlock(IndexProperty.PropertyType, Sections.Skip(1));
                    return;
                }
                catch (Exception)
                {
                }
            }

            if (typeof(IEnumerable).IsAssignableFrom(ParentType))
            {

            }
        }

        public PropertyInfo GetProperty(string name)
           => IncludeProperties
           ? ParentType.GetProperties().FirstOrDefault(x => CompareName(x, name))
           : null;

        public MethodInfo GetMethod(string name)
            => IncludeMethods
            ? ParentType.GetMethods()
                .Where(x =>
                {
                    var parameters = x.GetParameters();
                    return (parameters.Length == 0 || parameters.All(p => p.HasDefaultValue)) &&
                        x.ReturnParameter != null;
                })
                .FirstOrDefault(x => CompareName(x, name))
            : null;

        public PropertyInfo GetIndexProperty()
            => IncludeIndexProperties
            ? ParentType.GetProperties().FirstOrDefault(x => x.GetIndexParameters().Length > 0)
            : null;

        protected bool CompareName(MemberInfo member, string name)
        {
            var comparisonType = CaseSensitive
                ? StringComparison.InvariantCulture
                : StringComparison.InvariantCultureIgnoreCase;

            if (AcceptDisplayName && member.GetDisplayName().Equals(name, comparisonType))
                return true;
            
            return member.Name.Equals(name, comparisonType);
        }
    }
}
