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
        return ReactiveCommand.CreateFromTask(OnExecuteAsync, GetCanExecute(), AvaloniaScheduler.Instance);
    }

    protected abstract Task OnExecuteAsync(CancellationToken cancellationToken);
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
        return ReactiveCommand.CreateFromTask<TParam?>(OnExecuteAsync, GetCanExecute(), AvaloniaScheduler.Instance);
    }

    protected abstract Task OnExecuteAsync(TParam? parameter, CancellationToken cancellationToken);

    protected internal override bool CanExecute(object? parameter)
       => CanExecute((TParam?)parameter);

    protected virtual bool CanExecute(TParam? parameter)
    {
        return true;
    }
}
