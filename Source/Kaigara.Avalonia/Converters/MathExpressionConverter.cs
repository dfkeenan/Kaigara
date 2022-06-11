using System.Globalization;
using Avalonia.Data.Converters;

namespace Kaigara.Avalonia.Converters;

public class MathExpressionConverter : IValueConverter
{
    public static readonly MathExpressionConverter Instance = new MathExpressionConverter();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string input && MathExpressionParser.TryParseExpression(input, out var expression))
        {
            var converterdValue = expression.Compile().Invoke();
            parameter = ConverterParameterBindingExtension.GetParameterValue(parameter);

            if (parameter is string format)
            {
                return format.Contains("{0") ? string.Format(culture, format, converterdValue) : converterdValue.ToString(format, culture);
            }

            return converterdValue.ToString(culture);
        }
        return value;
    }
}
