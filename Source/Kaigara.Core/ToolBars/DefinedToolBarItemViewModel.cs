using System.Windows.Input;
using Avalonia.Input;
using ReactiveUI;

namespace Kaigara.ToolBars;

internal class DefinedToolBarItemViewModel : ReactiveObject, IToolBarItemViewModel
{
    private IDisposable changeSubscription;

    public DefinedToolBarItemViewModel(ToolBarItemDefinition definition)
    {
        Definition = definition ?? throw new System.ArgumentNullException(nameof(definition));

        changeSubscription = definition.Changed.Subscribe(n =>
        {
            this.RaisePropertyChanged(n.PropertyName);
        });
    }

    public ToolBarItemDefinition Definition { get; }
    public string Name => Definition.Name;
    public string? Label => Definition.Label;
    public string? IconName => Definition.IconName;
    public virtual bool IsVisible => Definition.IsVisible;
    public virtual ICommand? Command => Definition.Command;
    public virtual KeyGesture? InputGesture => Definition.InputGesture;
    public virtual object? CommandParameter => null;

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        changeSubscription.Dispose();
    }
}
