using System.Collections;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using Autofac;
using ReactiveUI;

namespace Kaigara.ToolBars;

public class ToolBarDefinition : ReactiveObject, IEnumerable<ToolBarItemDefinition>, IUIComponentDefinition<ToolBarItemDefinition>, IUIComponentDefinition
{
    private readonly ObservableCollection<ToolBarItemDefinition> items;
    private List<Action<IComponentContext>>? bindings;
    private CompositeDisposable disposables;
    private bool isVisible;
    public ToolBarDefinition(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        items = new ObservableCollection<ToolBarItemDefinition>();
        Items = Collections.ObjectModel.ReadOnlyObservableCollectionExtensionsHelpers.AsReadOnlyObservableCollection<ToolBarItemDefinition>(items);
        disposables = new CompositeDisposable();
        isVisible = true;
    }

    public string Name { get; }

    public bool IsVisible
    {
        get => isVisible;
        set => this.RaiseAndSetIfChanged(ref isVisible, value);
    }

    public ReadOnlyObservableCollection<ToolBarItemDefinition> Items { get; }

    IEnumerable<IUIComponentDefinition> IUIComponentDefinition.Items => items;

    void IUIComponentDefinition.OnParentDefined(IUIComponentDefinition parent)
        => ((ToolBarTrayDefinition)parent).Add(this);

    public void Add(ToolBarItemDefinition definition)
    {
        items.Add(definition);
    }

    public bool Remove(ToolBarItemDefinition definition)
    {
        return items.Remove(definition);
    }

    IEnumerator<ToolBarItemDefinition> IEnumerable<ToolBarItemDefinition>.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    internal virtual IToolBarViewModel Build()
        => new DefinedToolBarViewModel(this);

    protected ICollection<Action<IComponentContext>> Bindings
    {
        get
        {
            bindings ??= new List<Action<IComponentContext>>();
            return bindings;
        }
    }

    public ToolBarDefinition VisibleWhen<TSource>(Func<TSource, IObservable<bool>> selector)
         where TSource : notnull
    {
        Bindings.Add(context =>
        {
            var source = context.Resolve<TSource>();
            selector(source).BindTo(this, o => o.IsVisible).DisposeWith(disposables);
            this.RaisePropertyChanged(nameof(isVisible));
        });

        return this;
    }

    public void UpdateBindings(IComponentContext context)
    {
        bindings?.ForEach(a => a(context));
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        disposables.Dispose();

    }
}
