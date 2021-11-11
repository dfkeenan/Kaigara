using System.Collections.Specialized;
using System.Reactive.Disposables;
using System.Reflection;
using Autofac;
using Kaigara.Commands;

namespace Kaigara.ToolBars
{
    public class ToolBarManager : IToolBarManager
    {
        private readonly Dictionary<string, ToolBarTrayDefinition> trays = new Dictionary<string, ToolBarTrayDefinition>();
        private readonly IComponentContext context;
        private readonly UIComponentGraph definitionRegistrations;
        public ToolBarManager(IComponentContext context, ICommandManager commandManager)
        {
            if (commandManager is null)
            {
                throw new ArgumentNullException(nameof(commandManager));
            }

            this.context = context ?? throw new ArgumentNullException(nameof(context));
            definitionRegistrations = new UIComponentGraph(context);

            foreach (var command in commandManager.Commands)
            {
                RegisterCommand(command);
            }

            if (commandManager.Commands is INotifyCollectionChanged changed)
            {
                changed.CollectionChanged += OnCommandsCollectionChanged;
            }
        }

        private void OnCommandsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var command in e.NewItems!.Cast<RegisteredCommandBase>())
                {
                    RegisterCommand(command);
                }
            }
        }

        private void RegisterCommand(RegisteredCommandBase command)
        {
            var definitionAttribute = command.GetType().GetCustomAttribute<ToolBarItemDefinitionAttribute>();

            if (definitionAttribute is { })
            {
                var definition = new ToolBarItemDefinition(definitionAttribute.Name, definitionAttribute.Label, definitionAttribute.IconName);
                definition.RegisteredCommand = command;
                Register(definitionAttribute.Location, definition);
            }
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
