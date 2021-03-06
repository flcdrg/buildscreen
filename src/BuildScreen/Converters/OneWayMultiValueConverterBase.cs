﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace BuildScreen.Converters
{
    public abstract class OneWayMultiValueConverterBase : IMultiValueConverter
    {
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}