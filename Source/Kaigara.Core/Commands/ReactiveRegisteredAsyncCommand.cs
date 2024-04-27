using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Kaigara.Commands;

public abstract class ReactiveRegisteredAsyncCommand : ReactiveRegisteredCommandBase
{
    public ReactiveRegisteredAsyncCommand()
       : base()
    {

    }

    public ReactiveRegisteredAsyncCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected ReactiveRegisteredAsyncCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {
    }

    protected override ICommand CreateCommand()
    {
        return ReactiveCommand.CreateFromTask(OnExecuteAsync, CanExecute, AvaloniaScheduler.Instance);
    }

    protected abstract Task OnExecuteAsync(CancellationToken cancellationToken);
}

public abstract class ReactiveRegisteredAsyncCommand<TParam> : ReactiveRegisteredCommandBase
{
    public ReactiveRegisteredAsyncCommand()
       : base()
    {

    }

    public ReactiveRegisteredAsyncCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected ReactiveRegisteredAsyncCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {

    }

    protected override ICommand CreateCommand()
    {
        return ReactiveCommand.CreateFromTask<TParam?>(OnExecuteAsync, CanExecute, AvaloniaScheduler.Instance);
    }

    protected abstract Task OnExecuteAsync(TParam? parameter, CancellationToken cancellationToken);
}
