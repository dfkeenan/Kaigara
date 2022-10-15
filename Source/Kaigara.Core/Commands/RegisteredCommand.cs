using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Input;
using Avalonia.Threading;
using ReactiveUI;

namespace Kaigara.Commands;

public abstract class RegisteredCommand : RegisteredCommandBase
{
    public RegisteredCommand()
        : base()
    {

    }

    protected RegisteredCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {

    }

    protected override ICommand CreateCommand()
    {
        var canExecute = GetCanExecute();
        CanExecute = canExecute ?? Observable.Return(true);
        return ReactiveCommand.Create(OnExecute, canExecute, AvaloniaScheduler.Instance);
    }

    protected abstract void OnExecute();
}

public abstract class RegisteredCommand<TParam> : RegisteredCommandBase
{
    public RegisteredCommand()
       : base()
    {

    }

    protected RegisteredCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {

    }

    protected override ICommand CreateCommand()
    {
        var canExecute = GetCanExecute();
        CanExecute = canExecute ?? Observable.Return(true);

        return ReactiveCommand.Create<TParam>(OnExecute, canExecute, AvaloniaScheduler.Instance);
    }

    protected abstract void OnExecute(TParam param);
}
