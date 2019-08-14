using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyCSharpLib.Wpf.Converters
{
    public class ToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => value?.ToString();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => DependencyProperty.UnsetValue;
    }
}
