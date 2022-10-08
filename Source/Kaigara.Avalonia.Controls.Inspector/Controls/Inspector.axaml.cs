using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Utilities;
using Kaigara.Collections.ObjectModel;

namespace Kaigara.Avalonia.Controls;

[TemplatePart("PART_NodeItems", typeof(InspectorItemsControl))]
public class Inspector : TemplatedControl, IWeakEventSubscriber<NotifyCollectionChangedEventArgs>
{
    private IEnumerable _items = new AvaloniaList<object>();

    public static readonly DirectProperty<Inspector, IEnumerable> ItemsProperty =
            AvaloniaProperty.RegisterDirect<Inspector, IEnumerable>(nameof(Items), o => o.Items, (o, v) => o.Items = v);

    public IEnumerable Items
    {
        get { return _items; }
        set { SetAndRaise(ItemsProperty, ref _items, value); }
    }

    private ObservableCollection<InspectorNode> nodes = new ObservableCollection<InspectorNode>();

    public static readonly DirectProperty<Inspector, IEnumerable<InspectorNode>> NodesProperty =
    AvaloniaProperty.RegisterDirect<Inspector, IEnumerable<InspectorNode>>(
        nameof(Nodes),
        o => o.Nodes);

    public IEnumerable<InspectorNode> Nodes
    {
        get { return nodes; }
    }

    public static readonly DirectProperty<Inspector, ReflectionContext> ReflectionContextProperty =
    AvaloniaProperty.RegisterDirect<Inspector, ReflectionContext>(
       nameof(ReflectionContext),
       o => o.ReflectionContext,
       (o, v) => o.ReflectionContext = v);

    private ReflectionContext reflectionContext = new InspectorReflectionContext();

    public ReflectionContext ReflectionContext
    {
        get { return reflectionContext; }
        set => SetAndRaise(ReflectionContextProperty, ref reflectionContext, value);
    }


    public static readonly DirectProperty<Inspector, IEnumerable<Assembly>?> SearchAssembliesProperty
    = AvaloniaProperty.RegisterDirect<Inspector, IEnumerable<Assembly>?>(nameof(SearchAssemblies), o => o.SearchAssemblies, (o, v) => o.SearchAssemblies = v);

    private IEnumerable<Assembly>? searchAssemblies = null;

    public IEnumerable<Assembly>? SearchAssemblies
    {
        get => searchAssemblies;
        set => SetAndRaise(SearchAssembliesProperty, ref searchAssemblies, value);
    }

    internal InspectorItemsControl? NodeItemsControl { get; private set; }

    public InspectorContext InspectorContext { get; }

    public static readonly RoutedEvent<InspectorNodeChangedEventArgs> InspectorNodeChangedEvent =
        RoutedEvent.Register<Inspector, InspectorNodeChangedEventArgs>(nameof(InspectorNodeChanged), RoutingStrategies.Bubble);

    public event EventHandler<InspectorNodeChangedEventArgs> InspectorNodeChanged
    {
        add { AddHandler(InspectorNodeChangedEvent, value); }
        remove { RemoveHandler(InspectorNodeChangedEvent, value); }
    }

    static Inspector()
    {
        ItemsProperty.Changed.AddClassHandler<Inspector>((x, e) => x.ItemsChanged(e));
    }

    public Inspector()
    {
        InspectorContext = new InspectorContext(this);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        NodeItemsControl = e.NameScope.Find<InspectorItemsControl>("PART_NodeItems");

        if (NodeItemsControl is not null && Items is not null)
        {
            foreach (var item in Items)
            {
                nodes.Add(CreateNode(item));
            }
        }
    }

    protected virtual void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var oldValue = e.OldValue as IEnumerable;
        var newValue = e.NewValue as IEnumerable;

        if (oldValue is INotifyCollectionChanged oldIncc)
        {
            WeakEvents.CollectionChanged.Unsubscribe(oldIncc, this);
            nodes.Clear();
        }

        if (newValue is INotifyCollectionChanged newIncc)
        {
            if (NodeItemsControl is not null)
            {
                foreach (var item in newValue)
                {
                    nodes.Add(CreateNode(item));
                }
            }
            WeakEvents.CollectionChanged.Subscribe(newIncc, this);
        }
    }

    void IWeakEventSubscriber<NotifyCollectionChangedEventArgs>.OnEvent(object? sender, WeakEvent ev, NotifyCollectionChangedEventArgs e)
    {
        if (NodeItemsControl is not null)
        {
            nodes.MapSourceCollectionChanged(e, (Func<object, InspectorNode>)(i => CreateNode(i)));
        }
    }

    private InspectorNode CreateNode(object item)
    {
        return InspectorContext.CreateObjectInspectorNode(item);
    }
}
