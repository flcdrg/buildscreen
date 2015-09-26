using System;
using System.Globalization;

namespace BuildScreen.Converters
{
    public class BuildProgressWidthMultiConverter : OneWayMultiValueConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ratio = (double)values[0];
            var multiplied = ratio * (double)values[1];
            return Math.Abs(ratio - 1.0) < 0.00001 ? 0 : multiplied;
        }
    }
}