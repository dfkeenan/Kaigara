using System.Collections;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using Autofac;
using Kaigara.Collections.ObjectModel;
using ReactiveUI;

namespace Kaigara.Toolbars;

public class ToolbarDefinition : ReactiveObject, IEnumerable<ToolbarItemDefinition>, IUIComponentDefinition<ToolbarItemDefinition>, IUIComponentDefinition
{
    private readonly ObservableCollection<ToolbarItemDefinition> items;
    private List<Action<IComponentContext>>? bindings;
    private CompositeDisposable disposables;
    private bool isVisible;
    public ToolbarDefinition(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        items = new SortedObservableCollection<ToolbarItemDefinition>(UIComponentItemDefinition<ToolbarItemDefinition>.DisplayOrderComparer); ;
        Items = Collections.ObjectModel.ObservableCollectionExtensions.AsReadOnlyObservableCollection<ToolbarItemDefinition>(items);
        disposables = new CompositeDisposable();
        isVisible = true;
    }

    public string Name { get; }

    public bool IsVisible
    {
        get => isVisible;
        set => this.RaiseAndSetIfChanged(ref isVisible, value);
    }

    public ReadOnlyObservableCollection<ToolbarItemDefinition> Items { get; }

    IEnumerable<IUIComponentDefinition> IUIComponentDefinition.Items => items;

    void IUIComponentDefinition.OnParentDefined(IUIComponentDefinition parent)
        => ((ToolbarTrayDefinition)parent).Add(this);

    public void Add(ToolbarItemDefinition definition)
    {
        items.Add(definition);
    }

    public bool Remove(ToolbarItemDefinition definition)
    {
        return items.Remove(definition);
    }

    IEnumerator<ToolbarItemDefinition> IEnumerable<ToolbarItemDefinition>.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    internal virtual IToolbarViewModel Build()
        => new DefinedToolbarViewModel(this);

    protected ICollection<Action<IComponentContext>> Bindings
    {
        get
        {
            bindings ??= new List<Action<IComponentContext>>();
            return bindings;
        }
    }

    public ToolbarDefinition VisibleWhen<TSource>(Func<TSource, IObservable<bool>> selector)
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
