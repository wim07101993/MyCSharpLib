using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace MyCSharpLib.Wpf.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is IEnumerable enumerable))
                return value.ToString();

            var list = enumerable.Cast<object>().ToList();
            if (list.Count <= 0)
                return null;

            var builder = new StringBuilder();
            foreach (var item in list.Take(list.Count - 1))
                builder.Append(item.ToString()).Append(", ");

            builder.Append(list.Last().ToString());
            return builder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => DependencyProperty.UnsetValue;
    }
}
