using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Kaigara.ToolBars
{
    public class ToolBarManager : IToolBarManager
    {
        private readonly Dictionary<string, ToolBarTrayDefinition> trays = new Dictionary<string, ToolBarTrayDefinition>();
        private readonly IComponentContext context;
        private readonly UIComponentGraph definitionRegistrations;
        public ToolBarManager(IComponentContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            definitionRegistrations = new UIComponentGraph(context);
        }

        public IDisposable ConfigureDefinition(ToolBarLocation location, Action<ToolBarDefinition> options)
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

        public IDisposable ConfigureDefinition(ToolBarItemLocation location, Action<ToolBarItemDefinition> options)
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

        public IDisposable Register(ToolBarTrayDefinition definition)
        {
            if (definition is null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (trays.ContainsKey(definition.Name))
            {
                throw new ArgumentException("ToolBarTray already registered.", nameof(definition));
            }

            trays.Add(definition.Name, definition);
            var location = new ToolBarLocation(definition.Name);
            var registration = definitionRegistrations.Add(definition);
            var itemDisposables = new CompositeDisposable(definition.Items.Select(d => Register(location, d)).ToList());

            return Disposable.Create(() =>
            {
                registration.Dispose();
                trays.Remove(definition.Name);
                itemDisposables.Dispose();
            });
        }

        public IDisposable Register(ToolBarLocation location, ToolBarDefinition definition)
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

        public IDisposable Register(ToolBarItemLocation location, ToolBarItemDefinition definition)
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
    }
}
