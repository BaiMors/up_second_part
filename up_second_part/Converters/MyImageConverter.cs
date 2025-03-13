using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace up_second_part.Converters
{
    internal class MyImageConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (value == null || value == "") ? new Bitmap(AssetLoader.Open(new Uri("avares://up_second_part/Assets/picture.png"))) : new Bitmap(AssetLoader.Open(new Uri($"avares://up_second_part/Assets/Товар_import/{value}")));
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return new Avalonia.Data.BindingNotification(value);
        }
    }
}
