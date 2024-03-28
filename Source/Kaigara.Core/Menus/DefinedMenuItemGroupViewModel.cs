using System.Windows.Input;
using Avalonia.Input;

namespace Kaigara.Menus;

internal class DefinedMenuItemGroupViewModel : DefinedMenuItemViewModel
{
    public DefinedMenuItemGroupViewModel(MenuItemGroupDefinition definition)
        : base(definition)
    {
    }
}

internal class MenuItemSeparatorViewModel : IMenuItemViewModel
{
    public static MenuItemSeparatorViewModel Instance { get; } = new MenuItemSeparatorViewModel();


    public string Name => "Separator";
    public string? Label => "-";

    public string? IconName => null;

    public int DisplayOrder => 0;

    public virtual bool IsVisible => true;

    public virtual ICommand? Command => null;
    public virtual KeyGesture? InputGesture => null;
    public virtual object? CommandParameter => null;
    public virtual IEnumerable<IMenuItemViewModel> Items => Enumerable.Empty<IMenuItemViewModel>();

    public void Dispose()
    {

    }
}
