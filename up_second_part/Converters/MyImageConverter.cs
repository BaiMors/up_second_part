using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System.Threading.Tasks;
using System.IO;
using Avalonia.Platform;

namespace work10_school.Converters
{
    internal class MyImageConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (value == null || value == "") ? new Bitmap(AssetLoader.Open(new Uri("avares://up_second_part/Assets/logo.png"))) : new Bitmap(AssetLoader.Open(new Uri($"avares://up_second_part/Assets/Товар_import/{value}")));
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
