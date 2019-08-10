﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Unity;

namespace MyCSharpLib.Wpf.Controls
{
    public abstract class AControl : Control, IWithUnityContainer
    {
        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty StringsProperty = DependencyProperty.Register(
            nameof(Strings),
            typeof(IControlsStrings),
            typeof(AControl));

        #endregion DEPENDENCY PROPERTIES
        

        #region CONSTRUCTORS

        protected AControl()
        {
            try
            {
                Strings = UnityContainer.Resolve<IControlsStrings>();
                Strings.PropertyChanged += OnStringsPropertyChanged;
            }
            catch (ResolutionFailedException e)
            {
                throw new AggregateException($"The {nameof(IControlsStrings)} interface must be resolvable to use the {GetType().Name} control.", e);
            }
        }

        #endregion CONSTRUCTORS


        #region PROPERTIES

        public IControlsStrings Strings
        {
            get => (IControlsStrings)GetValue(StringsProperty);
            private set => SetValue(StringsProperty, value);
        }

        public IUnityContainer UnityContainer
        {
            get
            {
                if  (!(Application.Current is IWithUnityContainer container))
                    throw new InvalidOperationException($"For this control to work, you need to implement the {nameof(IWithUnityContainer)} interface in the application");

                return container.UnityContainer;
            }
        }

        #endregion PROPERTIES


        #region METHODS

        protected virtual void OnStringsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        protected T GetTemplateChild<T>(string childName)
            where T : class
        {
            if (!(GetTemplateChild(childName) is T child))
                throw new InvalidOperationException($"You failed to specify the {childName} in the template.");
            return child;
        }

        #endregion METHODS
    }
}