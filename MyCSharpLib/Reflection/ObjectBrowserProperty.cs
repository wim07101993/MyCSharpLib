﻿using Prism.Mvvm;
using System.Reflection;
using MyCSharpLib.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyCSharpLib.Reflection
{
    public class ObjectBrowserProperty : BindableBase
    {
        #region FIELDS

        private object _parent;

        #endregion FIELDS


        #region CONSTRUCTORS

        public ObjectBrowserProperty(PropertyInfo property, object parent)
        {
            Parent = parent;
            Property = property;

            Type = Property.PropertyType;

            var displayAttribute = Property.GetCustomAttribute<DisplayAttribute>();

            DisplayName = Property.GetDisplayName() ?? displayAttribute?.Name;
            Description = Property.GetDescription() ?? displayAttribute?.Description;
            IsBrowsable = Property.IsBrowsable();
            IsReadOnly = !Property.CanWrite;
            EditableState = Property.GetEditableState();
            DefaultValue = Property.GetDefaultValue();
            AuthorizedLevels = Property.GetAuthorizationLevels();
            IsEnum = Type.IsEnum || Property.GetCustomAttribute<EnumDataTypeAttribute>() != null;

            ValidationAttributes = Type.GetCustomAttributes<ValidationAttribute>().ToArray();
        }

        #endregion CONSTRUCTORS


        #region PROPERTIES

        public object Parent
        {
            get => _parent;
            set
            {
                if (_parent != null && Type.IsAssignableFrom(_parent.GetType()))
                    throw new InvalidOperationException("Cannot set parent to different type");

                if (!SetProperty(ref _parent, value))
                    return;

                RaisePropertyChanged(nameof(Value));
                RaisePropertyChanged(nameof(TypeProperties));
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

                Property.SetValue(Parent, value.Cast(Type));
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
        public bool IsFlagEnum => Property.GetCustomAttributes<FlagsAttribute>().Any();

        public string[] EnumNames
            => IsEnum
                ? Enum.GetNames(Property.PropertyType)
                : null;

        public IEnumerable<ObjectBrowserProperty> TypeProperties
            => Type.GetProperties()
                .Where(x => x.CanRead)
                .Select(x => new ObjectBrowserProperty(x, Value));

        public IEnumerable<ValidationAttribute> ValidationAttributes { get; }

        #endregion PROPERTIES
    }
}
