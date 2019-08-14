using MyCSharpLib.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MyCSharpLib.Wpf.Controls.ObjectBrowser
{
    public class ObjectBrowser : AControl
    {
        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            nameof(Type),
            typeof(Type),
            typeof(ObjectBrowser),
            new PropertyMetadata(default(Type), OnTypeChanged));

        public static readonly DependencyProperty OverrideTypeProperty = DependencyProperty.Register(
            nameof(OverrideType),
            typeof(bool?),
            typeof(ObjectBrowser),
            new PropertyMetadata(false, OnOverrideTypeChanged));

        public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register(
            nameof(Properties),
            typeof(IEnumerable<ObjectBrowserProperty>),
            typeof(ObjectBrowser),
            new PropertyMetadata(null, OnPropertiesChanged));

        public static readonly DependencyProperty VisiblePropertiesProperty = DependencyProperty.Register(
            nameof(VisibleProperties),
            typeof(IEnumerable<ObjectBrowserProperty>),
            typeof(ObjectBrowser));

        public static readonly DependencyProperty AuthorizedLevelProperty = DependencyProperty.Register(
            nameof(AuthorizedLevel),
            typeof(int?),
            typeof(ObjectBrowser),
            new PropertyMetadata(null, OnAuthorizedLevelChanged));

        public static readonly DependencyProperty AuthorizationDelegateProperty = DependencyProperty.Register(
           nameof(AuthorizationDelegate),
           typeof(AuthorizationDelegate),
           typeof(ObjectBrowser),
           new PropertyMetadata(null, OnAuthorizationDelegateChanged));

        #endregion DEPENDENCY PROPERTIES


        #region CONSTRUCTORS

        static ObjectBrowser()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ObjectBrowser), new FrameworkPropertyMetadata(typeof(ObjectBrowser)));
        }

        public ObjectBrowser()
        {
            DataContextChanged += OnDataContextChanged;
        }

        #endregion CONSTRUCTORS


        #region PROPERTIES

        [Bindable(true)]
        public Type Type
        {
            get => (Type)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        [Bindable(true)]
        public bool? OverrideType
        {
            get => (bool?)GetValue(OverrideTypeProperty);
            set => SetValue(OverrideTypeProperty, value);
        }

        public IEnumerable<ObjectBrowserProperty> Properties
        {
            get => (IEnumerable<ObjectBrowserProperty>)GetValue(PropertiesProperty);
            private set => SetValue(PropertiesProperty, value);
        }

        public IEnumerable<ObjectBrowserProperty> VisibleProperties
        {
            get => (IEnumerable<ObjectBrowserProperty>)GetValue(PropertiesProperty);
            private set => SetValue(PropertiesProperty, value);
        }

        [Bindable(true)]
        public int? AuthorizedLevel
        {
            get => (int?)GetValue(AuthorizedLevelProperty);
            set => SetValue(AuthorizedLevelProperty, value);
        }

        [Bindable(true)]
        public AuthorizationDelegate AuthorizationDelegate
        {
            get => (AuthorizationDelegate)GetValue(AuthorizationDelegateProperty);
            set => SetValue(AuthorizationDelegateProperty, value);
        }

        #endregion PROPERTIES


        #region METHODS

        #region callbacks

        private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ObjectBrowser browser) || Equals(e.NewValue, e.OldValue))
                return;

            browser.Properties = browser.Type
                .GetProperties(BindingFlags.Public)
                .Where(x => x.CanRead)
                .Select(x => new ObjectBrowserProperty(x));
        }

        private static void OnOverrideTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ObjectBrowser browser) || Equals(e.NewValue, e.OldValue))
                return;

            if (browser.OverrideType != true)
                browser.Type = browser.DataContext.GetType();
        }

        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ObjectBrowser browser) || Equals(e.NewValue, e.OldValue))
                return;

            browser.UpdateVisibleProperties();
        }

        private static void OnAuthorizedLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ObjectBrowser browser) || Equals(e.NewValue, e.OldValue))
                return;

            browser.UpdateVisibleProperties();
        }

        private static void OnAuthorizationDelegateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ObjectBrowser browser) || Equals(e.NewValue, e.OldValue))
                return;

            browser.UpdateVisibleProperties();
        }

        #endregion callbacks

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (OverrideType == true)
                Type = DataContext.GetType();
        }

        public void UpdateVisibleProperties()
        {
            if (Properties == null)
            {
                VisibleProperties = null;
                return;
            }

            var props = Properties.Where(x => x.IsBrowsable);

            if (AuthorizationDelegate != null)
            {
                var level = AuthorizedLevel;
                props = AuthorizedLevel == null
                    ? props.Where(x => x.AuthorizedLevels == 0)
                    : props.Where(x => (x.AuthorizedLevels & level) > 0);
            }

            VisibleProperties = props;
        }

        #endregion METHODS
    }
}
