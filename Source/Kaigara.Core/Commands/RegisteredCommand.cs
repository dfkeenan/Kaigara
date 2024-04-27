using System.Windows.Input;
using Avalonia.Input;
using Kaigara.Input;

namespace Kaigara.Commands;

public abstract class RegisteredCommand : RegisteredCommandBase
{
    private IRelayCommand? relayCommand;

    public RegisteredCommand()
       : base()
    {

    }

    public RegisteredCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected RegisteredCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {
    }

    protected virtual bool CanExecute() => true;

    protected abstract void OnExecute();

    protected void NotifyCanExecuteChanged()
    {
        if (relayCommand is null) return;
        relayCommand.NotifyCanExecuteChanged();
        //Dispatcher.UIThread.Invoke(relayCommand.NotifyCanExecuteChanged);
    }

    protected override ICommand CreateCommand()
    {
        relayCommand = new RelayCommand(OnExecute,CanExecute);

        return relayCommand;
    }
}

public abstract class RegisteredCommand<TParam> : RegisteredCommandBase
{
    private IRelayCommand? relayCommand;
    public RegisteredCommand()
       : base()
    {

    }

    public RegisteredCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected RegisteredCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {
    }

    protected virtual bool CanExecute(TParam? parameter) => true;

    protected abstract void OnExecute(TParam? parameter);

    protected void NotifyCanExecuteChanged()
    {
        if ( relayCommand is null ) return;
        relayCommand.NotifyCanExecuteChanged();
        //Dispatcher.UIThread.Invoke(relayCommand.NotifyCanExecuteChanged);
    }

    protected override ICommand CreateCommand()
    {
        relayCommand = new RelayCommand<TParam>(OnExecute, CanExecute);
        
        return relayCommand;
    }
}
