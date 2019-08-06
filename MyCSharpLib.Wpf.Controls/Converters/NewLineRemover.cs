﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace MyCSharpLib.Wpf.Converters
{
    public class NewLineRemover : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Replace('\r', ',').Replace('\n',' ');
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}