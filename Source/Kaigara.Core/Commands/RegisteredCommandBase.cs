using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using Avalonia.Input;
using ReactiveUI;

namespace Kaigara.Commands;

public abstract class RegisteredCommandBase : ReactiveObject
{
    public RegisteredCommandBase()
    {
        var thisType = GetType();
        Name = thisType.FullName!;
        label = thisType.Name;

        if (thisType.GetCustomAttribute<CommandDefinitionAttribute>() is CommandDefinitionAttribute attribute)
        {
            if (attribute.DefaultInputGesture is string gesture)
            {
                keyGesture = KeyGesture.Parse(gesture);
            }

            Name = attribute.Name ?? Name;
            Label = attribute.Label;
            IconName = attribute.IconName;
        }
    }

    protected RegisteredCommandBase(string name, string label, KeyGesture? keyGesture, string? iconName)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        this.label = label ?? throw new ArgumentNullException(nameof(label));
        this.iconName = iconName;
        this.keyGesture = keyGesture;
    }

    public string Name { get; }

    private string label;
    public string Label
    {
        get => label;
        set => this.RaiseAndSetIfChanged(ref label, value);
    }
    private string? iconName;
    public string? IconName
    {
        get => iconName;
        set => this.RaiseAndSetIfChanged(ref iconName, value);
    }

    private KeyGesture? keyGesture;
    private ICommand? command;

    public KeyGesture? InputGesture
    {
        get => keyGesture;
        set => this.RaiseAndSetIfChanged(ref keyGesture, value);
    }

    public ICommand Command => command ??= CreateCommand();

    public IObservable<bool>? CanExecute { get; protected set; }

    protected virtual IObservable<bool> GetCanExecute() => Observable.Return(true);
    protected abstract ICommand CreateCommand();
}
