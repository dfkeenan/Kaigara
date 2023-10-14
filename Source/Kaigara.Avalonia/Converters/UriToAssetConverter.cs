using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Platform;

namespace Kaigara.Avalonia.Converters;

public class UriToAssetConverter : IValueConverter
{
    public static UriToAssetConverter Instance { get; } = new UriToAssetConverter();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Uri uri && AssetLoader.Exists(uri) == true)
        {
            var stream = AssetLoader.Open(uri);
            try
            {
                var result = Activator.CreateInstance(targetType, stream);
                return result ?? AvaloniaProperty.UnsetValue;
            }
            catch
            {

            }
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
