using Prism.Mvvm;
using System.Reflection;
using MyCSharpLib.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;

namespace MyCSharpLib.Reflection
{
    public class ObjectBrowserProperty : BindableBase
    {
        #region FIELDS

        private object _parent;

        #endregion FIELDS


        #region CONSTRUCTORS

        public ObjectBrowserProperty(PropertyInfo property)
        {
            Property = property;

            Type = Property.PropertyType;

            DisplayName = Property.GetDisplayName();
            Description = Property.GetDescription();
            IsBrowsable = Property.IsBrowsable();
            IsReadOnly = !Property.CanWrite;
            EditableState = Property.GetEditableState();
            DefaultValue = Property.GetDefaultValue();
            AuthorizedLevels = Property.GetAuthorizationLevels();
            IsEnum = Type.IsEnum;
        }

        #endregion CONSTRUCTORS


        #region PROPERTIES

        public object Parent
        {
            get => _parent;
            set
            {
                if (!SetProperty(ref _parent, value))
                    return;

                RaisePropertyChanged(nameof(Value));
            }
        }

        public object Value
        {
            get => Parent == null
                ? null
                : Property?.GetValue(_parent);
            set
            {
                if (Parent == null)
                    return;

                Property.SetValue(Parent, value);
            }
        }

        public PropertyInfo Property { get; }

        public Type Type { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public bool IsBrowsable { get; }
        public bool IsReadOnly { get; }
        public int AuthorizedLevels { get; }
        public EditorBrowsableState EditableState { get; }
        public object DefaultValue { get; }

        public double Accuracy => Property.GetCustomAttribute<AccuracyAttribute>()?.Accuracy ?? default;

        public bool IsEnum { get; }
        public bool IsFlagEnum => Property.GetCustomAttribute<FlagsAttribute>() != null;

        public string[] EnumNames
            => IsEnum
                ? Enum.GetNames(Property.PropertyType)
                : null;

        public IEnumerable<ObjectBrowserProperty> TypeProperties
            => Type.GetProperties(BindingFlags.Public)
                .Where(x => x.CanRead)
                .Select(x => new ObjectBrowserProperty(x));

        #endregion PROPERTIES
    }
}
