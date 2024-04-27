using System.Windows.Input;
using Avalonia.Input;
using Kaigara.Commands;
using ReactiveUI;

namespace Kaigara.Menus;

internal class DefinedMenuItemViewModelBase : ReactiveObject, IDisposable, IMenuItemViewModel
{
    public DefinedMenuItemViewModelBase(MenuItemDefinition definition)
    {
        this.Definition = definition ?? throw new ArgumentNullException(nameof(definition));
    }

    public MenuItemDefinition Definition { get; }

    public string Name => Definition.Name;
    public string? Label => Definition.Label;

    public string? IconName => Definition.IconName;

    public int DisplayOrder => Definition.DisplayOrder;

    public virtual bool IsVisible => Definition.IsVisible;

    public virtual ICommand? Command => Definition.Command;
    public CanExecuteBehavior CanExecuteBehavior => Definition.CanExecuteBehavior;
    public virtual KeyGesture? InputGesture => Definition.InputGesture;
    public virtual IEnumerable<IMenuItemViewModel> Items => Enumerable.Empty<IMenuItemViewModel>();

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {

    }
}
