using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace EmployeeManager.Desktop.Converters
{
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
                return (DateTimeOffset?)new DateTimeOffset(dateTime);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
                return dateTimeOffset.DateTime;
            return null;
        }
    }
}
