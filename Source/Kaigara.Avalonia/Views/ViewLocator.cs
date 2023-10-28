using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Kaigara.Avalonia.Views;

public class ViewLocator : IDataTemplate
{
    private readonly Dictionary<string, Type> viewTypeCache = new Dictionary<string, Type>();
    public bool SupportsRecycling => false;

    public Control Build(object? data)
    {
        if(data == null) return new TextBlock { Text = "Not Found" };

        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        Type? type = GetType(name);

        if (type != null)
        {
            var control = (Control)Activator.CreateInstance(type)!;
            ViewLocated(data, control);
            return control;
        }
        else
        {
            return new TextBlock { Text = "Not Found: " + name };
        }
    }

    protected virtual void ViewLocated(object data, Control control)
    {
        
    }

    private Type? GetType(string name)
    {
        if (viewTypeCache.TryGetValue(name, out var type))
        {
            return type;
        }

        type = Type.GetType(name) ?? AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => TryGetTypes(a))
            .FirstOrDefault(t => t.FullName == name);

        if (type is { })
        {
            viewTypeCache[name] = type;
        }

        return type;

        static IEnumerable<Type> TryGetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (Exception)
            {
                return Enumerable.Empty<Type>();
            }
        }
    }

    public bool Match(object? data)
    {
        return data is { } && data.GetType().Name.EndsWith("ViewModel");
    }
}
