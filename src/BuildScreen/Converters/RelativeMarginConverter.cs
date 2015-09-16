using System;
using System.Globalization;
using System.Windows;

namespace BuildScreen.Converters
{
    public class RelativeMarginConverter : OneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness((double)value * double.Parse((string)parameter));
        }
    }
}