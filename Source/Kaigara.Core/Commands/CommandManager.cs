using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Utilities;
using DynamicData.Alias;
using ReactiveUI;

namespace Kaigara.Commands;

public class CommandManager : ICommandManager
{
    private static CommandManager? Instance;

    static CommandManager()
    {
        //HACK: This is terrible, find another way
        InputElement.GotFocusEvent.AddClassHandler<TopLevel>((t, e) =>
        {
            Instance?.requerySuggested.Invoke(Instance, EventArgs.Empty);
        }, handledEventsToo: true);
    }

    private readonly ObservableCollection<RegisteredCommandBase> commands;

    public CommandManager(IEnumerable<RegisteredCommandBase> commands)
    {
        if (commands is null)
        {
            throw new ArgumentNullException(nameof(commands));
        }

        RequerySuggested = Observable.FromEventPattern<Action<object?, EventArgs>, EventArgs>(e => requerySuggested += e, e => requerySuggested -= e)
                                                .Select(p => p.EventArgs)
                                                .Throttle(TimeSpan.FromMilliseconds(500))
                                                .ObserveOn(RxApp.MainThreadScheduler);
        
        Instance ??= this;

        this.commands = new ObservableCollection<RegisteredCommandBase>(commands);

        Commands = Collections.ObjectModel.ObservableCollectionExtensions.AsReadOnlyObservableCollection<RegisteredCommandBase>(this.commands);

        foreach (var command in this.commands)
        {
            command.OnRegistered(this);
        }

        this.commands.CollectionChanged += Commands_CollectionChanged;
    }

    private void Commands_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (var command in e.NewItems?.Cast<RegisteredCommandBase>() ?? [])
                {
                    command?.OnRegistered(this);
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                break;
            case NotifyCollectionChangedAction.Replace:
                break;
            case NotifyCollectionChangedAction.Move:
                break;
            case NotifyCollectionChangedAction.Reset:
                break;
        }
    }

    public ReadOnlyObservableCollection<RegisteredCommandBase> Commands { get; }

    private event Action<object, EventArgs> requerySuggested = default!;

    event EventHandler<EventArgs> ICommandManager.RequerySuggestedEvent
    {
        add => WeakEventHandlerManager.Subscribe<CommandManager, EventArgs, Action<object, EventArgs>>(this, nameof(requerySuggested), value);
        remove => WeakEventHandlerManager.Unsubscribe<EventArgs, Action<object, EventArgs>>(this, nameof(requerySuggested), value);
    }

    public IObservable<EventArgs> RequerySuggested { get; }
}
