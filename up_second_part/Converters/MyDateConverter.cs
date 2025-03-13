using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace up_second_part.Converters
{
    internal class MyDateConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime date) return new DateTimeOffset(date);
            return null;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset date) return new DateTime(date.Year, date.Month, date.Day);
            return null;
        }
    }
}
