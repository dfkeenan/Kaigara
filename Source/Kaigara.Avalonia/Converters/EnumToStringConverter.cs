using System.Globalization;
using Avalonia.Data.Converters;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Converters;

public class EnumToStringConverter : IValueConverter
{
    public static EnumToStringConverter Instance { get; } = new EnumToStringConverter();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var valueType = value!.GetType().EnsureRuntimeType();

        var fieldInfo = valueType.GetField(value.ToString()!);
        return fieldInfo!.GetDisplayName();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
