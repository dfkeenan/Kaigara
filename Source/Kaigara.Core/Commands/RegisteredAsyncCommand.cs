using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Kaigara.Commands;

public abstract class RegisteredAsyncCommand : RegisteredCommandBase
{
    public RegisteredAsyncCommand()
       : base()
    {

    }

    protected RegisteredAsyncCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {
    }

    protected override ICommand CreateCommand()
    {
        var canExecute = GetCanExecute();
        CanExecute = canExecute ?? Observable.Return(true);
        return ReactiveCommand.CreateFromTask(OnExecuteAsync, canExecute, AvaloniaScheduler.Instance);
    }

    protected abstract Task OnExecuteAsync();
}

public abstract class RegisteredAsyncCommand<TParam> : RegisteredCommandBase
{
    public RegisteredAsyncCommand()
       : base()
    {

    }

    protected RegisteredAsyncCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {

    }

    protected override ICommand CreateCommand()
    {
        var canExecute = GetCanExecute();
        CanExecute = canExecute ?? Observable.Return(true);
        return ReactiveCommand.CreateFromTask<TParam>(OnExecuteAsync, canExecute, AvaloniaScheduler.Instance);
    }

    protected abstract Task OnExecuteAsync(TParam param);
}
