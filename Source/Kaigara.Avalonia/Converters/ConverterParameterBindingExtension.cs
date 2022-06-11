using Avalonia;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace Kaigara.Avalonia.Converters;

public class ConverterParameterBindingExtension
{
    public ConverterParameterBindingExtension()
    {

    }

    public ConverterParameterBindingExtension(string path)
    {
        Path = path;
    }

    [ConstructorArgument("path")]
    public string? Path { get; set; }

    public RelativeSource? RelativeSource { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        var parameter = new ConverterParameterBinding();

        var binding = new ReflectionBindingExtension(Path) { RelativeSource = RelativeSource }.ProvideValue(serviceProvider);

        parameter.Bind(ConverterParameterBinding.ValueProperty, binding);

        return parameter;
    }

    public static object? GetParameterValue(object? parameter)
    {
        if (parameter is ConverterParameterBinding p)
        {
            return p.Value;
        }

        return parameter;
    }

    private class ConverterParameterBinding : AvaloniaObject
    {
        public static readonly StyledProperty<object> ValueProperty =
                        AvaloniaProperty.Register<ConverterParameterBinding, object>(nameof(Value));

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
