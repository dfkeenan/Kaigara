﻿using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Styling;

namespace Kaigara.Avalonia.Converters;

public class NameToResourceConverter : IValueConverter
{
    private IResourceHost? resourceHost;

    private IResourceHost? ResourceHost => resourceHost ?? Application.Current;

    public NameToResourceConverter(IResourceHost? resourceHost = null)
    {
        this.resourceHost = resourceHost;
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string name && ResourceHost?.TryFindResource(name, Application.Current?.ActualThemeVariant, out var resource) == true)
        {
            return resource!;
        }

        return AvaloniaProperty.UnsetValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ThemedNameToResourceConverter : IMultiValueConverter
{
    public static ThemedNameToResourceConverter Instance { get; } = new();

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if(values is [string name, IResourceHost resourceHost, ThemeVariant theme])
        {
            
            if (resourceHost.TryFindResource(name, theme, out var resource) == true)
            {
                return resource!;
            }
        }

        return AvaloniaProperty.UnsetValue;
    }
}


public class NameToResourceConverterExtension
{
    public NameToResourceConverterExtension()
    {

    }

    public IValueConverter ProvideValue(IServiceProvider serviceProvider)
    {
        var anchor = GetFirstParent<StyledElement>() ??
                    GetFirstParent<IResourceProvider>() ??
                    (object?)GetFirstParent<IResourceHost>();

        return new NameToResourceConverter(anchor as IResourceHost);

        T? GetFirstParent<T>() where T : class
        {
            return (serviceProvider.GetService(typeof(IAvaloniaXamlIlParentStackProvider)) as IAvaloniaXamlIlParentStackProvider)?
                .Parents.OfType<T>().FirstOrDefault();
        }
    }
}