using System;
using System.Globalization;
using System.Windows.Media;
using BuildScreen.ContinousIntegration.Entities;

namespace BuildScreen.Converters
{
    public class StatusToBrushConverter : OneWayValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Status)value)
            {
                case Status.Success: return new SolidColorBrush(Colors.DarkGreen);
                case Status.Fail: return new SolidColorBrush(Colors.DarkRed);
                default: return new SolidColorBrush(Colors.Gray);
            }
        }
    }
}