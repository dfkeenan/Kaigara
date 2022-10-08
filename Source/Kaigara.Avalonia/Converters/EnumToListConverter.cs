using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;
using Kaigara.Extentions;

namespace Kaigara.Avalonia.Converters;

public class EnumToListConverter : IValueConverter
{
    public static EnumToListConverter Instance { get; } = new EnumToListConverter();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var intValue = (long)value;
        var values = EnumExtentions.GetFlagsValues(value.GetType())
            .Cast<long>()
            .Where(v => (v & intValue) > 0)
            .Select(e => Enum.ToObject(targetType, e))
            .ToList();
        return values;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Enum.ToObject(targetType, (value as IEnumerable).Cast<long>().Aggregate(0L, (v, n) => v | n));
    }
}
