using System;
using System.Globalization;
using System.Windows;

namespace BuildScreen.Converters
{
    public class EqualToOneToVisibleOrHiddenConverter : OneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Abs((double)value - 1.0) < 0.00001 ? Visibility.Visible : Visibility.Hidden;
        }
    }
}