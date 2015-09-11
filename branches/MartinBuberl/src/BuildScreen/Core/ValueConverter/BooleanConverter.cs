using System;
using System.Globalization;
using System.Windows.Data;

namespace BuildScreen.Core.ValueConverter
{
    [ValueConversion(typeof(bool?), typeof(bool))]
    public class BooleanConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool param = bool.Parse(parameter.ToString());

            return value != null && (bool)value == param;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool param = bool.Parse(parameter.ToString());

            return (bool)value == param;
        }

        #endregion
    }
}
