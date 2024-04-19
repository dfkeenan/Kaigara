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

    public RegisteredAsyncCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected RegisteredAsyncCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {
    }

    protected override ICommand CreateCommand()
    {
        return ReactiveCommand.CreateFromTask(OnExecuteAsync, CanExecute, AvaloniaScheduler.Instance);
    }

    protected abstract Task OnExecuteAsync();
}

public abstract class RegisteredAsyncCommand<TParam> : RegisteredCommandBase
{
    public RegisteredAsyncCommand()
       : base()
    {

    }

    public RegisteredAsyncCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected RegisteredAsyncCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {

    }

    protected override ICommand CreateCommand()
    {
        return ReactiveCommand.CreateFromTask<TParam>(OnExecuteAsync, CanExecute, AvaloniaScheduler.Instance);
    }

    protected abstract Task OnExecuteAsync(TParam param);
}
