﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace MyCSharpLib.Wpf.Converters
{
    public class EditorBrowsableStateToBooleanConverter : IValueConverter
    {
        public static EditorBrowsableStateToBooleanConverter Instance { get; } = new EditorBrowsableStateToBooleanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is EditorBrowsableState state && state != EditorBrowsableState.Never;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value is bool b && b
            ? EditorBrowsableState.Always
            : EditorBrowsableState.Never;
    }
}