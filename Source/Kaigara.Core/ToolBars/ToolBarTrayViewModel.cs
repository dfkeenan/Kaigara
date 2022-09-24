using System.Collections.ObjectModel;

namespace Kaigara.ToolBars;

public class ToolBarTrayViewModel : IDisposable
{
    private readonly ToolBarTrayDefinition definition;
    private ReadOnlyObservableCollection<IToolBarViewModel> items;

    public ToolBarTrayViewModel(ToolBarTrayDefinition definition)
    {
        this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
        items = Collections.ObjectModel.ObservableCollectionExtensions.ToReadOnlyObservableCollectionOf<ToolBarDefinition, IToolBarViewModel>(definition.Items, (Func<ToolBarDefinition, IToolBarViewModel>)(d => d.Build()));
    }

    public IEnumerable<IToolBarViewModel> Items => items;

    public ToolBarTrayDefinition Definition => definition;

    public void Dispose()
    {
        foreach (var item in items)
        {
            item.Dispose();
        }
    }
}
