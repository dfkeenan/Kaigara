using System.Collections.ObjectModel;

namespace Kaigara.Toolbars;

public class ToolbarTrayViewModel : IDisposable
{
    private readonly ToolbarTrayDefinition definition;
    private ReadOnlyObservableCollection<IToolbarViewModel> items;

    public ToolbarTrayViewModel(ToolbarTrayDefinition definition)
    {
        this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
        items = Collections.ObjectModel.ObservableCollectionExtensions.ToReadOnlyObservableCollectionOf<ToolbarDefinition, IToolbarViewModel>(definition.Items, (Func<ToolbarDefinition, IToolbarViewModel>)(d => d.Build()));
    }

    public IEnumerable<IToolbarViewModel> Items => items;

    public ToolbarTrayDefinition Definition => definition;

    public void Dispose()
    {
        foreach (var item in items)
        {
            item.Dispose();
        }
    }
}
