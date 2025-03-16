using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace EmployeeManager.Desktop.Utils
{
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime.Year < 1000)
                    return null;

                return new DateTimeOffset(dateTime);
            }
            return null;
        }


        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                if (dateTimeOffset.Year < 1000)
                    return null;

                return dateTimeOffset.UtcDateTime;
            }
            return null;
        }
    }
}
