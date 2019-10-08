using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
        public PropertyInfo GenericIdProperty { get; set; }
        public MethodInfo GenericIdMethod { get; set; }


        private void BuildNextBlock()
        {
            var section = Section;
            if (TryGetProperty(section))
            {
                NextBlock = new ExpressionBuildBlock(Property.PropertyType, Sections.Skip(1));
                return;
            }
            
            if (TryGetMethod(section))
            {
                NextBlock = new ExpressionBuildBlock(Method.ReturnType, Sections.Skip(1));
                return;
            }
            
            if (TryGetIndexProperty() && TryGetIndexParameter(section))
            {
                NextBlock = new ExpressionBuildBlock(IndexProperty.PropertyType, Sections.Skip(1));
                return;
            }

            if (TryGetGenericIdMember())
            {
                NextBlock = new ExpressionBuildBlock(GenericIdProperty?.PropertyType ?? GenericIdMethod.ReturnType, Sections.Skip(1));
                return;
            }
        }

        private bool TryGetProperty(string name)
        {
            if (!IncludeProperties)
                return false;

            Property = ParentType.GetProperties()
                .FirstOrDefault(x => CompareName(x, name));

            return Property != null;
        }

        private bool TryGetMethod(string name)
        {
            if (!IncludeMethods)
                return false;

            Method = ParentType.GetMethods()
                .Where(x =>
                {
                    var parameters = x.GetParameters();
                    return (parameters.Length == 0 || parameters.All(p => p.HasDefaultValue)) &&
                        x.ReturnParameter != null;
                })
                .FirstOrDefault(x => CompareName(x, name));

            return Method != null;
        }

        private bool TryGetIndexProperty()
        {
            if (!IncludeIndexProperties)
                return false;

            IndexProperty = ParentType.GetProperties()
                .FirstOrDefault(x => x.GetIndexParameters().Length > 0);

            return IndexProperty != null;
        }

        private bool TryGetIndexParameter(string value)
        {
            Parameter = IndexProperty.GetIndexParameters().First();
            try
            {
                ParameterValue = Convert.ChangeType(value, Parameter.ParameterType);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        } 

        private bool TryGetGenericIdMember()
        {
            if (!typeof(IEnumerable<>).IsAssignableFrom(ParentType))
                return false;

            var genericType = GetGenericArgumentType();
            var genericProperties = genericType.GetProperties();

            var idRegex = new Regex(Regexamples.IdRegex);
            if (TryGetGenericIdProperty(genericProperties, idRegex))
                return true;

            var nameRegex = new Regex(Regexamples.NameRegex);
            if (TryGetGenericIdProperty(genericProperties, nameRegex))
                return true;

            var genericMethods = genericType.GetMethods()
                .Where(x => x.ReturnType != null && x.GetParameters().Length == 0)
                .ToArray();

            if (TryGetGenericIdMethod(genericMethods, idRegex))
                return true;
            if (TryGetGenericIdMethod(genericMethods, nameRegex))
                return true;

            return false;
        }

        private bool TryGetGenericIdProperty(PropertyInfo[] properties, Regex regex)
        {
            GenericIdProperty = properties.FirstOrDefault(x =>
            {
                if (AcceptDisplayName && regex.IsMatch(x.GetDisplayName()))
                    return true;
                return regex.IsMatch(x.Name);
            });

            return GenericIdProperty != null;
        }

        private bool TryGetGenericIdMethod(MethodInfo[] methods, Regex regex)
        {
            GenericIdMethod = methods.FirstOrDefault(x =>
            {
                if (AcceptDisplayName && regex.IsMatch(x.GetDisplayName()))
                    return true;
                return regex.IsMatch(x.Name);
            });

            return GenericIdProperty != null;
        }

        private Type GetGenericArgumentType()
        {
            var type = ParentType;

            while (type != typeof(IEnumerable<>))
                type = type.GetInterfaces().First(x => typeof(IEnumerable<>).IsAssignableFrom(ParentType));

            return type.GetGenericArguments().First();
        }

        private bool CompareName(MemberInfo member, string name)
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
