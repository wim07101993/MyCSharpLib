using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyCSharpLib.Wpf.Converters
{
    public class NewLineRemover : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var str = value.ToString()
                .Replace('\r', ',')
                .Replace('\n', ' ')
                .Replace('\t', ' ')
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            return string.Join(" ", str);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => DependencyProperty.UnsetValue;
    }
}
