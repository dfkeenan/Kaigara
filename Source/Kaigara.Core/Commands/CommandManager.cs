using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Utilities;
using ReactiveUI;

namespace Kaigara.Commands;

public class CommandManager : ICommandManager, IDisposable
{
    private readonly ObservableCollection<RegisteredCommandBase> commands;
    private CompositeDisposable? disposables;
    public CommandManager()
    {
        

        disposables = new CompositeDisposable();

        RequerySuggested = Observable.FromEventPattern<Action<object?, EventArgs>, EventArgs>(e => requerySuggested += e, e => requerySuggested -= e)
                                                .Select(p => p.EventArgs)
                                                .Throttle(TimeSpan.FromMilliseconds(500))
                                                .ObserveOn(RxApp.MainThreadScheduler);

        commands = [];

        Commands = Collections.ObjectModel.ObservableCollectionExtensions.AsReadOnlyObservableCollection(commands);

        //HACK: This is terrible, find another way
        InputElement.GotFocusEvent.AddClassHandler<TopLevel>((t, e) =>
        {
            requerySuggested.Invoke(this, EventArgs.Empty);
        }, handledEventsToo: true).DisposeWith(disposables);
    }

    public void Dispose()
    {
        disposables?.Dispose();
        disposables = null;
        commands.Clear();
    }

    public ReadOnlyObservableCollection<RegisteredCommandBase> Commands { get; }

    private event Action<object, EventArgs> requerySuggested = default!;

    public event EventHandler<EventArgs> RequerySuggestedEvent
    {
        add => WeakEventHandlerManager.Subscribe<CommandManager, EventArgs, Action<object, EventArgs>>(this, nameof(requerySuggested), value);
        remove => WeakEventHandlerManager.Unsubscribe<EventArgs, Action<object, EventArgs>>(this, nameof(requerySuggested), value);
    }

    public IObservable<EventArgs> RequerySuggested { get; }

    public void RegisterCommand(RegisteredCommandBase command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (commands.Contains(command))
        {
            return;
        }

        commands.Add(command);
        command.OnRegistered(this);
    }
}
