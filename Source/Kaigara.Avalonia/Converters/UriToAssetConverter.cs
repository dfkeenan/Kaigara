using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Platform;

namespace Kaigara.Avalonia.Converters
{
    public class UriToAssetConverter : IValueConverter
    {
        public static UriToAssetConverter Instance { get; } = new UriToAssetConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IAssetLoader assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>();
            if (value is Uri uri && assetLoader.Exists(uri))
            {
                var stream = assetLoader.Open(uri);
                try
                {
                    var result = Activator.CreateInstance(targetType, stream);
                    return result ?? AvaloniaProperty.UnsetValue;
                }
                catch
                {

                }
            }

            return  AvaloniaProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
