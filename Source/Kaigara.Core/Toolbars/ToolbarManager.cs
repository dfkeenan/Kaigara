using System.Collections.Specialized;
using System.Reactive.Disposables;
using System.Reflection;
using Autofac;
using Kaigara.Commands;

namespace Kaigara.Toolbars;

public class ToolbarManager : IToolbarManager
{
    private readonly Dictionary<string, ToolbarTrayDefinition> trays = new Dictionary<string, ToolbarTrayDefinition>();
    private readonly IComponentContext context;
    private readonly UIComponentGraph definitionRegistrations;
    public ToolbarManager(IComponentContext context, ICommandManager commandManager)
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
        var definitionAttribute = command.GetType().GetCustomAttribute<ToolbarItemDefinitionAttribute>();

        if (definitionAttribute is not null)
        {
            var definition = definitionAttribute.GetDefinition(command)!;

            Register(definitionAttribute.Location, definition);
        }
        else if (command is IToolbarItemDefinitionSource source && source.IsDefined)
        {
            var definition = source.GetDefinition(command)!;

            Register(source.Location, definition);
        }
    }

    public IDisposable ConfigureDefinition(ToolbarLocation location, Action<ToolbarDefinition> options)
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

    public IDisposable ConfigureDefinition(ToolbarItemLocation location, Action<ToolbarItemDefinition> options)
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

    public IDisposable Register(ToolbarTrayDefinition definition)
    {
        if (definition is null)
        {
            throw new ArgumentNullException(nameof(definition));
        }

        if (trays.ContainsKey(definition.Name))
        {
            throw new ArgumentException("ToolbarTray already registered.", nameof(definition));
        }

        trays.Add(definition.Name, definition);
        var location = new ToolbarLocation(definition.Name);
        var registration = definitionRegistrations.Add(definition);
        var itemDisposables = new CompositeDisposable(definition.Items.Select(d => Register(location, d)).ToList());

        return Disposable.Create(() =>
        {
            registration.Dispose();
            trays.Remove(definition.Name);
            itemDisposables.Dispose();
        });
    }

    public IDisposable Register(ToolbarLocation location, ToolbarDefinition definition)
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

    public IDisposable Register(ToolbarItemLocation location, ToolbarItemDefinition definition)
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
