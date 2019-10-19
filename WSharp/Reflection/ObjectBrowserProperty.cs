using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using WSharp.Extensions;

namespace WSharp.Reflection
{
    /// <summary>
    ///     Class that is used to browse through an object in abstraction. It holds all the values
    ///     neede to let the user browse through an object.
    /// </summary>
    public class ObjectBrowserProperty : BindableBase
    {
        #region FIELDS

        private object _parent;

        #endregion FIELDS

        #region CONSTRUCTORS

        /// <summary>
        ///     Constructs a new <see cref="ObjectBrowserProperty"/> from a
        ///     <see cref="PropertyInfo"/> and its parent.
        /// </summary>
        /// <param name="property">Information about the property.</param>
        /// <param name="parent">Parent of the property.</param>
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

            ValidationAttributes = Property.GetCustomAttributes<ValidationAttribute>().ToArray();

            if (Parent is INotifyPropertyChanged notifyPropertyChanged)
                notifyPropertyChanged.PropertyChanged += OnParentPropertyChanged;
        }

        #endregion CONSTRUCTORS

        #region PROPERTIES

        /// <summary>The parent to which this property belongs.</summary>
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

        /// <summary>Value of the property</summary>
        public object Value
        {
            get => Parent == null
                ? null
                : Property?.GetValue(_parent);
            set
            {
                if (Parent == null)
                    return;

                Property.SetValue(Parent, value.CastObject(Type));
            }
        }

        /// <summary>Basic information about the property info.</summary>
        public PropertyInfo Property { get; }

        /// <summary>Type of the propertie's value.</summary>
        public Type Type { get; }

        /// <summary>Display name of the property.</summary>
        public string DisplayName { get; }

        /// <summary>Description of the property.</summary>
        public string Description { get; }

        /// <summary>Indicates whether the property is overall browsable.</summary>
        public bool IsBrowsable { get; }

        /// <summary>Indicates whether it it possible to write to the property.</summary>
        public bool IsReadOnly { get; }

        /// <summary>Levels that are allowed to access the value.</summary>
        public int AuthorizedLevels { get; }

        /// <summary>Indicate whether the property should be visible in the editor.</summary>
        public EditorBrowsableState EditableState { get; }

        /// <summary>The default value of the property.</summary>
        public object DefaultValue { get; }

        /// <summary>The accurracy at which a value should be displayed.</summary>
        public double Accuracy => Property.GetCustomAttribute<AccuracyAttribute>()?.Accuracy ?? default;

        /// <summary>Indicates whether this property is an enum.</summary>
        public bool IsEnum { get; }

        /// <summary>
        ///     Indicate whether the type of enum (if this property is an enum) is a flagged enum.
        /// </summary>
        public bool IsFlagEnum => Property.GetCustomAttributes<FlagsAttribute>().Any();

        /// <summary>Nameof of the enum type (if the type is an enum).</summary>
        public string[] EnumNames
            => IsEnum
                ? Enum.GetNames(Property.PropertyType)
                : null;

        /// <summary>Properties of the property.</summary>
        public IEnumerable<ObjectBrowserProperty> TypeProperties
            => Type.GetProperties()
                .Where(x => x.CanRead && x.GetIndexParameters().Length == 0)
                .Select(x => new ObjectBrowserProperty(x, Value));

        /// <summary>Attributes that validate the value of the property.</summary>
        public IEnumerable<ValidationAttribute> ValidationAttributes { get; }

        #endregion PROPERTIES

        #region METHODS

        private void OnParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Property.Name)
                RaisePropertyChanged(nameof(Value));
        }

        #endregion METHODS
    }
}