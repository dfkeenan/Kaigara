using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Kaigara.Commands;

public abstract class ReactiveRegisteredCommand : ReactiveRegisteredCommandBase
{
    public ReactiveRegisteredCommand()
        : base()
    {

    }

    public ReactiveRegisteredCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected ReactiveRegisteredCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {

    }

    protected override ICommand CreateCommand()
    {
        return ReactiveCommand.Create(OnExecute, CanExecute, AvaloniaScheduler.Instance);
    }

    protected abstract void OnExecute();
}

public abstract class ReactiveRegisteredCommand<TParam> : ReactiveRegisteredCommandBase
{
    public ReactiveRegisteredCommand()
       : base()
    {

    }

    public ReactiveRegisteredCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected ReactiveRegisteredCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {

    }

    protected override ICommand CreateCommand()
    {
        return ReactiveCommand.Create<TParam>(OnExecute, CanExecute, AvaloniaScheduler.Instance);
    }

    protected abstract void OnExecute(TParam? parameter);
}
