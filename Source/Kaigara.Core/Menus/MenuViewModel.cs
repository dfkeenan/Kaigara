using System.Collections.ObjectModel;
using Kaigara.Collections.ObjectModel;

namespace Kaigara.Menus;

public class MenuViewModel : IDisposable
{
    private readonly MenuDefinition definition;
    private ReadOnlyObservableCollection<IMenuItemViewModel> items;

    public MenuViewModel(MenuDefinition definition)
    {
        this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
        items = definition.Items.ToReadOnlyObservableCollectionOf(d => d.Build());
    }

    public IEnumerable<IMenuItemViewModel> Items => items;

    public MenuDefinition Definition => definition;

    public void Dispose()
    {
        foreach (var item in items)
        {
            item.Dispose();
        }
    }
}
