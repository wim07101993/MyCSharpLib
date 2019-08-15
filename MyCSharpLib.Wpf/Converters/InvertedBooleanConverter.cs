using System;
using System.Globalization;
using System.Windows.Data;

namespace MyCSharpLib.Wpf.Converters
{
    public class InvertedBooleanConverter : IValueConverter
    {
        public static InvertedBooleanConverter Instance { get; } = new InvertedBooleanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b && !b;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b && !b;
    }
}
