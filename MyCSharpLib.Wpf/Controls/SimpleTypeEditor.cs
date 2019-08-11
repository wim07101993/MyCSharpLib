using System;
using System.Windows;

namespace MyCSharpLib.Wpf.Controls
{
    public class SimpleTypeEditor : AControl
    {
        #region ROUTED EVENTS

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ValueChanged),
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<object>),
            typeof(NumericUpDown));

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
        }

        private static void OnOverrideTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is SimpleTypeEditor simpleTypeEditor) ||
                  Equals(e.NewValue, e.OldValue))
                return;

            simpleTypeEditor.UpdateType();
        }

        #endregion callbacks

        public void UpdateType()
        {
            if (Value != null && (Type == null || !OverrideType))
                Type = Value.GetType();
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
