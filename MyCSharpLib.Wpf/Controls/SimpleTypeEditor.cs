using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MyCSharpLib.Wpf.Controls
{
    public class SimpleTypeEditor : AControl
    {
        #region ROUTED EVENTS

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ValueChanged),
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<object>),
            typeof(SimpleTypeEditor));

        #endregion ROUTED EVENTS


        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(object),
            typeof(SimpleTypeEditor),
            new PropertyMetadata(default, OnValueChanged));

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            nameof(Type),
            typeof(Type),
            typeof(SimpleTypeEditor),
            new PropertyMetadata(default(Type), OnTypeChanged));

        public static readonly DependencyProperty OverrideTypeProperty = DependencyProperty.Register(
            nameof(OverrideType),
            typeof(bool),
            typeof(SimpleTypeEditor),
            new PropertyMetadata(default(bool), OnOverrideTypeChanged));

        public static readonly DependencyProperty TemplatesProperty = DependencyProperty.Register(
            nameof(Templates),
            typeof(IDictionary<Type, ControlTemplate>),
            typeof(SimpleTypeEditor),
            new PropertyMetadata(new Dictionary<Type, ControlTemplate>()
            {
                { typeof(DateTime), (ControlTemplate)Application.Current.TryFindResource("DateTimeEditorTemplate") },
                { typeof(TimeSpan), (ControlTemplate)Application.Current.TryFindResource("TimeSpanEditorTemplate") },
                { typeof(Enum), (ControlTemplate)Application.Current.TryFindResource("EnumEditorTemplate") },
                { typeof(bool), (ControlTemplate)Application.Current.TryFindResource("BoolEditorTemplate") },
                { typeof(string), (ControlTemplate)Application.Current.TryFindResource("StringEditorTemplate") },
                { typeof(char), (ControlTemplate)Application.Current.TryFindResource("CharEditorTemplate") },
                { typeof(byte), (ControlTemplate)Application.Current.TryFindResource("ByteEditorTemplate") },
                { typeof(sbyte), (ControlTemplate)Application.Current.TryFindResource("SByteEditorTemplate") },
                { typeof(short), (ControlTemplate)Application.Current.TryFindResource("ShortEditorTemplate") },
                { typeof(ushort), (ControlTemplate)Application.Current.TryFindResource("UShortEditorTemplate") },
                { typeof(int), (ControlTemplate)Application.Current.TryFindResource("IntEditorTemplate") },
                { typeof(uint), (ControlTemplate)Application.Current.TryFindResource("UIntEditorTemplate") },
                { typeof(long), (ControlTemplate)Application.Current.TryFindResource("LongEditorTemplate") },
                { typeof(ulong), (ControlTemplate)Application.Current.TryFindResource("ULongEditorTemplate") },
                { typeof(float), (ControlTemplate)Application.Current.TryFindResource("FloatEditorTemplate") },
                { typeof(decimal), (ControlTemplate)Application.Current.TryFindResource("DecimalEditorTemplate") },
                { typeof(double), (ControlTemplate)Application.Current.TryFindResource("DoubleEditorTemplate") },
            }, OnTemplatesChanged));

        #endregion DEPENDENCY PROPERTIES


        #region CONSTRUCTORS

        static SimpleTypeEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleTypeEditor), new FrameworkPropertyMetadata(typeof(SimpleTypeEditor)));
        }

        #endregion CONSTRUCTORS


        #region PROPERTIES

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public Type Type
        {
            get => (Type)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public bool OverrideType
        {
            get => (bool)GetValue(OverrideTypeProperty);
            set => SetValue(OverrideTypeProperty, value);
        }

        public IDictionary<Type, ControlTemplate> Templates
        {
            get => (IDictionary<Type, ControlTemplate>)GetValue(TemplatesProperty);
        }

        #endregion PROPERTIES


        #region METHODS

        #region callbacks

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is SimpleTypeEditor simpleTypeEditor) ||
                Equals(e.NewValue, e.OldValue))
                return;

            simpleTypeEditor.UpdateType();
            simpleTypeEditor.RaiseEvent(new RoutedPropertyChangedEventArgs<object>(e.OldValue, e.NewValue, ValueChangedEvent));
        }

        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is SimpleTypeEditor simpleTypeEditor) ||
                 Equals(e.NewValue, e.OldValue))
                return;

            simpleTypeEditor.UpdateTemplate();
        }

        private static void OnOverrideTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is SimpleTypeEditor simpleTypeEditor) ||
                  Equals(e.NewValue, e.OldValue))
                return;

            simpleTypeEditor.UpdateType();
        }

        private static void OnTemplatesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is SimpleTypeEditor simpleTypeEditor) ||
                Equals(e.NewValue, e.OldValue))
                return;

            simpleTypeEditor.UpdateTemplate();
        }

        #endregion callbacks

        public void UpdateType()
        {
            if (Value != null && (Type == null || !OverrideType))
                Type = Value.GetType();
        }

        public void UpdateTemplate()
        {
            var type = Value.GetType();
            Template = 
                Templates.FirstOrDefault(x => x.Key == type && x.Value != null).Value ??
                Templates.FirstOrDefault(x => type.IsAssignableFrom(x.Key) && x.Value != null).Value;
        }

        #endregion METHODS


        #region EVENTS

        public event RoutedPropertyChangedEventHandler<object> ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        #endregion EVENTS
    }
}
