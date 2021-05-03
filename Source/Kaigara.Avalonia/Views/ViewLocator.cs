using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace Kaigara.Avalonia.Views
{
    public class ViewLocator : IDataTemplate
    {
        private readonly Dictionary<string, Type> viewTypeCache = new Dictionary<string, Type>();
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            Type? type = GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        private Type? GetType(string name)
        {
            if(viewTypeCache.TryGetValue(name, out var type))
            {
                return type;
            }

            type = Type.GetType(name) ?? AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == name);

            if (type is { })
            {
                viewTypeCache[name] = type;
            }

            return type;
        }

        public bool Match(object data)
        {
            return data is { } && data.GetType().Name.EndsWith("ViewModel");
        }
    }
}
