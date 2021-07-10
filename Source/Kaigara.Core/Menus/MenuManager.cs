using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Kaigara.Commands;

namespace Kaigara.Menus
{
    public class MenuManager : IMenuManager
    {
        private readonly Dictionary<string, MenuDefinition> menus = new Dictionary<string, MenuDefinition>();
        private readonly UIComponentGraph definitionRegistrations;
        private readonly IComponentContext context;

        public MenuManager(IComponentContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            definitionRegistrations = new UIComponentGraph(context);
        }

        public IDisposable ConfigureDefinition(MenuItemLocation location, Action<MenuItemDefinition> options)
        {
            if (location is null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return definitionRegistrations.AddConfiguration(location, options);
        }

        public IDisposable Register(MenuItemLocation location, MenuItemDefinition definition)
        {
            if (location is null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            if (definition is null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            return definitionRegistrations.Add(location, definition);
        }

        public IDisposable Register(MenuDefinition definition)
        {
            if (definition is null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (menus.ContainsKey(definition.Name))
            {
                throw new ArgumentException("Menu already registered.", nameof(definition));
            }

            menus.Add(definition.Name, definition);
            var location = new MenuItemLocation(definition.Name);
            var registration = definitionRegistrations.Add(definition);

            var itemDisposables = new CompositeDisposable( definition.Items.Select(d => Register(location, d)).ToList());
            
            return Disposable.Create(()=>
            {
                registration.Dispose();
                menus.Remove(definition.Name);
                itemDisposables.Dispose();
            });
        }

        public MenuViewModel BuildMenu(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if(menus.TryGetValue(name, out var definition))
            {
                return new MenuViewModel(definition);
            }

            throw new ArgumentException($"There is no menu registered with name '{name}'.", nameof(name));
        }

        public IDisposable BuildMenu(MenuDefinition definition, out MenuViewModel menu)
        {
            var registration = Register(definition);
            menu = new MenuViewModel(definition);
            return registration;
        }
    }
}
