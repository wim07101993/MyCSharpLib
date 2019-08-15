using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyCSharpLib.Wpf.Converters
{
    public class InvertedBooleanToVisibilityConverter : IValueConverter
    {
        public static InvertedBooleanToVisibilityConverter Instance { get; } = new InvertedBooleanToVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => value is bool b && b
                ? Visibility.Collapsed
                : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Visibility v && v == Visibility.Collapsed;
    }
}
