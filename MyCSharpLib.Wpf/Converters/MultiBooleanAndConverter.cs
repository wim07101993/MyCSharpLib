using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MyCSharpLib.Wpf.Converters
{
    public class MultiBooleanAndConverter : IMultiValueConverter
    {
        public static MultiBooleanAndConverter Instance { get; } = new MultiBooleanAndConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            => values?.All(x => x is bool b && b) == true;

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => new[] { DependencyProperty.UnsetValue };
    }
}
