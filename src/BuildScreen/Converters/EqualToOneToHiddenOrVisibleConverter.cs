using System;
using System.Globalization;
using System.Windows;

namespace BuildScreen.Converters
{
    public class EqualToOneToHiddenOrVisibleConverter : OneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Abs((double)value - 1.0) < 0.00001 ? Visibility.Hidden : Visibility.Visible;
        }
    }
}