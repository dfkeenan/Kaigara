using System.Windows.Input;
using Avalonia.Input;
using Avalonia.Threading;
using Kaigara.Input;

namespace Kaigara.Commands;
public abstract class RegisteredAyncCommand : RegisteredCommandBase
{
    private IRelayCommand? relayCommand;

    public RegisteredAyncCommand()
       : base()
    {

    }

    public RegisteredAyncCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected RegisteredAyncCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {
    }

    protected virtual bool CanExecute() => true;

    protected abstract Task OnExecute();

    protected void NotifyCanExecuteChanged()
    {
        if (relayCommand is null) return;
        relayCommand.NotifyCanExecuteChanged();
        //Dispatcher.UIThread.Invoke(relayCommand.NotifyCanExecuteChanged);
    }

    protected override ICommand CreateCommand()
    {
        relayCommand = new AsyncRelayCommand(OnExecute, CanExecute);

        return relayCommand;
    }
}

public abstract class RegisteredAyncCommand<TParam> : RegisteredCommandBase
{
    private IRelayCommand? relayCommand;
    public RegisteredAyncCommand()
       : base()
    {

    }

    public RegisteredAyncCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected RegisteredAyncCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {
    }

    protected virtual bool CanExecute(TParam? parameter) => true;

    protected abstract Task OnExecute(TParam? parameter);

    protected void NotifyCanExecuteChanged()
    {
        if (relayCommand is null) return;
        relayCommand.NotifyCanExecuteChanged();
        //Dispatcher.UIThread.Invoke(relayCommand.NotifyCanExecuteChanged);
    }

    protected override ICommand CreateCommand()
    {
        relayCommand = new AsyncRelayCommand<TParam>(OnExecute, CanExecute);

        return relayCommand;
    }
}
