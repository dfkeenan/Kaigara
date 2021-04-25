using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace Kaigara.Avalonia.Converters
{
    public class WindowIconToBitmapConverter : IValueConverter
    {
        public static WindowIconToBitmapConverter Instance { get; } = new WindowIconToBitmapConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WindowIcon icon)
            {
                using var stream = new MemoryStream();

                icon.Save(stream);
                stream.Position = 0;

                var bitmap = new Bitmap(stream);

                return bitmap;
            }

            return AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
