using System.Globalization;
using Avalonia.Data.Converters;
using Kaigara.Avalonia.Extensions;

namespace Kaigara.Avalonia.Converters;

public class TypeToSimpleNameConverter : IValueConverter
{
    public static TypeToSimpleNameConverter Instance { get; } = new TypeToSimpleNameConverter();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (value as Type)?.ToSimpleCSharpName() ?? "Type Unknown";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
