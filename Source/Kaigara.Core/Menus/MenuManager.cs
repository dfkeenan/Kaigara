using System.Collections.Specialized;
using System.Reactive.Disposables;
using System.Reflection;
using Autofac;
using Kaigara.Commands;

namespace Kaigara.Menus;

public class MenuManager : IMenuManager
{
    private readonly Dictionary<string, MenuDefinition> menus = new Dictionary<string, MenuDefinition>();
    private readonly UIComponentGraph definitionRegistrations;
    private readonly IComponentContext context;

    public MenuManager(IComponentContext context, ICommandManager commandManager)
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
        var definitionAttribute = command.GetType().GetCustomAttribute<MenuItemDefinitionAttribute>();

        if (definitionAttribute is { })
        {
            var definition = new MenuItemDefinition(definitionAttribute.Name, definitionAttribute.Label, definitionAttribute.IconName);
            definition.RegisteredCommand = command;
            Register(definitionAttribute.Location, definition);
        }
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

        var itemDisposables = new CompositeDisposable(definition.Items.Select(d => Register(location, d)).ToList());

        return Disposable.Create(() =>
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

        if (menus.TryGetValue(name, out var definition))
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
