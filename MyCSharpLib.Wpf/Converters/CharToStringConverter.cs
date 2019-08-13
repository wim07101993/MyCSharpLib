using System;
using System.Globalization;
using System.Windows.Data;

namespace MyCSharpLib.Wpf.Converters
{
    public class CharToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value.ToString();

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string s) ||
                !string.IsNullOrEmpty(s))
                return default(char);

            return s[0];
        }
    }
}
