using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Kaigara.Avalonia.Converters;

public class NameToResourceConverter : IValueConverter
{
    public static NameToResourceConverter Instance { get; } = new NameToResourceConverter();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string name && Application.Current.TryFindResource(name, out var resource))
        {
            return resource!;
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
