using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using Kaigara.Reflection;
using ReactiveUI;

namespace Kaigara.Avalonia.Controls.InspectorNodes;

public class CollectionInspectorNode : ObjectInspectorNodeBase
{
    private readonly PropertyInfo itemProperty;
    private readonly InspectorNodeProvider itemNodeProvider;

    private readonly ObservableCollection<MemberInspectorNode> items;

    public CollectionInspectorNode(object instance, InspectorContext context, InspectorNodeProvider provider, InspectorNode parent, MemberInfo memberInfo, string displayName)
        : base(instance, context, provider, parent, memberInfo, displayName)
    {

        itemProperty = Type.GetProperty("Item")!;

        itemNodeProvider = Context.GetNodeProvider(itemProperty);

        items = new ObservableCollection<MemberInspectorNode>();

        Items = new ReadOnlyObservableCollection<MemberInspectorNode>(items);

        IsReadOnly = List.IsReadOnly;

        var canAdd = itemProperty.PropertyType.CanBeActivatedWithoutArguments();

        AddNewItem = ReactiveCommand.Create(AddNewListItem, Observable.Never<bool>().StartWith(canAdd));

        for (int i = 0; i < List.Count; i++)
        {
            items.Add(CreateItemNode(List, i));
        }

        items.CollectionChanged += Items_CollectionChanged;
    }

    public bool IsReadOnly { get; }

    private Type ElementType => itemProperty.PropertyType;
    private IList List => (IList)Value;

    public ReadOnlyObservableCollection<MemberInspectorNode> Items { get; }

    public ReactiveCommand<Unit, Unit> AddNewItem { get; }    

    internal void RemoveListItem(MemberInspectorNode item)
    {
        int index = (int)item.GetIndex();
        items.RemoveAt(index);
    }

    private void AddNewListItem()
    {
        var item = Activator.CreateInstance(ElementType);
        var itemIndex = Items.Count;
        List.Add(item);
        items.Add(CreateItemNode(Value, itemIndex));
    }

    private MemberInspectorNode CreateItemNode(object list, int i)
    {
        return (MemberInspectorNode)itemNodeProvider.CreateNode(Context, this, itemProperty, new object[] { i });
    }

    public override IEnumerable<InspectorNode> Children => Items.AsEnumerable();

    public override void Invalidate()
    {
        items.CollectionChanged -= Items_CollectionChanged;

        items.Clear();

        for (int i = 0; i < List.Count; i++)
        {
            items.Add(CreateItemNode(Value, i));
        }

        items.CollectionChanged += Items_CollectionChanged;
    }

    private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Remove:
                {
                    var inspectorNodeChangedEventArgs = new InspectorNodeChangedEventArgs(this, Value, instanceEventArgs: ConvertEventArgs(e));
                    List.RemoveAt(e.OldStartingIndex);

                    for (int i = e.OldStartingIndex; i < Items.Count; i++)
                    {
                        Items[i].SetIndex(i);
                    }
                    Context.OnInspectorNodeChanged(this, inspectorNodeChangedEventArgs);
                }
                break;
            case NotifyCollectionChangedAction.Replace:
                //MemberInspectorNode already handles this.
                break;
            case NotifyCollectionChangedAction.Add:
            case NotifyCollectionChangedAction.Move:
            case NotifyCollectionChangedAction.Reset:
            default:
                {
                    var inspectorNodeChangedEventArgs = new InspectorNodeChangedEventArgs(this, Value, instanceEventArgs: ConvertEventArgs(e));
                    Context.OnInspectorNodeChanged(this, inspectorNodeChangedEventArgs);
                }
                break;
        }


    }

    internal static NotifyCollectionChangedEventArgs ConvertEventArgs(NotifyCollectionChangedEventArgs args)
    {
        NotifyCollectionChangedEventArgs result;

        var newItems = args.NewItems?.Cast<MemberInspectorNode>().Select(n => n.GetValue());
        var oldItems = args.OldItems?.Cast<MemberInspectorNode>().Select(n => n.GetValue());

        switch (args.Action)
        {
            case NotifyCollectionChangedAction.Add:
                result = new NotifyCollectionChangedEventArgs(args.Action, newItems!.First(), args.NewStartingIndex);
                break;
            case NotifyCollectionChangedAction.Remove:
                result = new NotifyCollectionChangedEventArgs(args.Action, oldItems!.First(), args.OldStartingIndex);
                break;
            case NotifyCollectionChangedAction.Replace:
                result = new NotifyCollectionChangedEventArgs(args.Action, newItems!.First(), oldItems!.First(), args.NewStartingIndex);
                break;
            case NotifyCollectionChangedAction.Move:
                result = new NotifyCollectionChangedEventArgs(args.Action, newItems!.ToList(), args.NewStartingIndex, args.OldStartingIndex);
                break;
            case NotifyCollectionChangedAction.Reset:
                result = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(args));
        }

        return result;
    }
}