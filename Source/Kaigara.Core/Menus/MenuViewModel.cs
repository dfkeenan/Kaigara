using System.Collections.ObjectModel;

namespace Kaigara.Menus;

public class MenuViewModel : IDisposable
{
    private readonly MenuDefinition definition;
    private ReadOnlyObservableCollection<IMenuItemViewModel> items;

    public MenuViewModel(MenuDefinition definition)
    {
        this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
        items = Collections.ObjectModel.ObservableCollectionExtensions.ToReadOnlyObservableCollectionOf<MenuItemDefinition, IMenuItemViewModel>(definition.Items, (Func<MenuItemDefinition, IMenuItemViewModel>)(d => d.Build()));
    }

    public ReadOnlyObservableCollection<IMenuItemViewModel> Items => items;

    public MenuDefinition Definition => definition;

    public void Dispose()
    {
        foreach (var item in items)
        {
            item.Dispose();
        }
    }
}
