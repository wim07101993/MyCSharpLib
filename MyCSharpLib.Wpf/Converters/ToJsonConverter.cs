using System;
using System.Globalization;
using System.Windows.Data;
using MyCSharpLib.Services.Serialization.Extensions;
using Newtonsoft.Json;

namespace MyCSharpLib.Wpf.Converters
{
    public class ToJsonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => value?.SerializeJson(Formatting.Indented);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => value?.ToString().DeserializeJson(targetType);
    }
}
