﻿using System;
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
        private readonly MenuGraph menuItemDefinitionRegistrations;
        private readonly IComponentContext context;

        public MenuManager(IComponentContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            menuItemDefinitionRegistrations = new MenuGraph(this);
        }

        internal IComponentContext ComponentContext => context;

        public IDisposable ConfigureMenuItemDefinition(MenuPath path, Action<MenuItemDefinition> options)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return menuItemDefinitionRegistrations.AddConfiguration(path, options);
        }

        public IDisposable Register(MenuPath path, MenuItemDefinition definition)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (definition is null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            return menuItemDefinitionRegistrations.Add(path, definition);
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
            var path = new MenuPath(definition.Name);

            var itemDisposables = new CompositeDisposable( definition.Items.Select(d => Register(path, d)).ToList());
            
            return Disposable.Create(()=>
            {
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
