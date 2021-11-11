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
        return ReactiveCommand.Create(OnExecute, CanExecute, AvaloniaScheduler.Instance);
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
        return ReactiveCommand.Create<TParam>(OnExecute, CanExecute, AvaloniaScheduler.Instance);
    }

    protected abstract void OnExecute(TParam param);
}
