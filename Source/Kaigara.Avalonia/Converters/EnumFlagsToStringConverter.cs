using System.Globalization;
using Avalonia.Data.Converters;
using Kaigara.Extentions;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Converters;

public class EnumFlagsToStringConverter : IValueConverter
{
    public static EnumFlagsToStringConverter Instance { get; } = new EnumFlagsToStringConverter();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var valueType = value!.GetType().EnsureRuntimeType();

        var displayValues = from v in EnumExtentions.GetFlagsValues(valueType, System.Convert.ToInt64(value))
                            let fieldInfo = valueType.GetField(v.ToString()!)
                            select fieldInfo.GetDisplayName();

        return string.Join(", ", displayValues) is string { Length: > 0 } converted ? converted : "None";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}