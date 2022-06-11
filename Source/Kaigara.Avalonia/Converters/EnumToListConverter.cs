using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Kaigara.Extentions;

namespace Kaigara.Avalonia.Converters;

public class EnumToListConverter : IValueConverter
{
    public static EnumToListConverter Instance { get; } = new EnumToListConverter();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var intValue = (int)value;
        var values = EnumExtentions.GetFlagsValues(value.GetType()).Cast<int>().Where(v => (v & intValue) > 0).ToList();
        return values;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (value as IEnumerable).Cast<int>().Aggregate(0, (v,n)=> v | n);
    }
}
