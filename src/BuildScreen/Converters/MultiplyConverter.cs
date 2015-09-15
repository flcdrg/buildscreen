using System;
using System.Globalization;

namespace BuildScreen.Converters
{
    public class MultiplyConverter : OneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * double.Parse((string)parameter);
        }
    }
}