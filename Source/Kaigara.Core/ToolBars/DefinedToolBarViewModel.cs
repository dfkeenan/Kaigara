using System.Collections.ObjectModel;
using ReactiveUI;

namespace Kaigara.ToolBars;

internal class DefinedToolBarViewModel : ReactiveObject, IToolBarViewModel
{
    private ReadOnlyObservableCollection<IToolBarItemViewModel> items;
    private IDisposable changeSubscription;

    public DefinedToolBarViewModel(ToolBarDefinition definition)
    {
        Definition = definition ?? throw new ArgumentNullException(nameof(definition));

        items = Collections.ObjectModel.ObservableCollectionExtensions.ToReadOnlyObservableCollectionOf<ToolBarItemDefinition, IToolBarItemViewModel>(definition.Items, (Func<ToolBarItemDefinition, IToolBarItemViewModel>)(d => d.Build()));

        changeSubscription = definition.Changed.Subscribe(n =>
        {
            this.RaisePropertyChanged(n.PropertyName);
        });
    }

    public ToolBarDefinition Definition { get; }

    public string Name => Definition.Name;

    public bool IsVisible => Definition.IsVisible;

    public IEnumerable<IToolBarItemViewModel> Items => items;

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        changeSubscription.Dispose();
    }
}
