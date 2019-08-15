using System.Collections.Generic;
using System.Windows;

namespace MyCSharpLib.Wpf.Controls
{
    public class DependencyObjectWrapper<T> : DependencyObject
    {
        public static readonly DependencyProperty ValuePoperty = DependencyProperty.Register(
            nameof(Value),
            typeof(T),
            typeof(DependencyObjectWrapper<T>));

        public T Value
        {
            get => (T)GetValue(ValuePoperty);
            set => SetValue(ValuePoperty, value);
        }
    }
}
