using System.Globalization;
using Avalonia.Data.Converters;

namespace Kaigara.Commands;

public enum CanExecuteBehavior
{
    Enabled,
    Visible
}

public class CanExecuteToIsVisibleConverer : IMultiValueConverter
{
    public static CanExecuteToIsVisibleConverer Instance { get; } = new();

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return values switch
        {
        [bool isVisible, bool isEnabled, CanExecuteBehavior behavior]
            => isVisible && behavior switch
            {
                CanExecuteBehavior.Enabled => true,
                CanExecuteBehavior.Visible => isEnabled,
                _ => true,
            },
            _ => true,
        };
    }
}