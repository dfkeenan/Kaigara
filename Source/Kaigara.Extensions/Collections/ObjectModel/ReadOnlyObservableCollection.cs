using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Kaigara.Collections.ObjectModel;

public class ReadOnlyObservableCollection<TSourceItem, TDestinationItem> : ReadOnlyObservableCollection<TDestinationItem>
{
    private readonly INotifyCollectionChanged source;
    private readonly Func<TSourceItem, TDestinationItem> converter;
    private readonly ObservableCollection<TDestinationItem> innerCollection;

    public ReadOnlyObservableCollection(ObservableCollection<TSourceItem> source, Func<TSourceItem, TDestinationItem> converter)
        : base(new ObservableCollection<TDestinationItem>(source.Select(model => converter(model))))
    {
        this.source = source;
        this.converter = converter;
        this.source.CollectionChanged += OnSourceCollectionChanged;
        innerCollection = (ObservableCollection<TDestinationItem>)this.Items;

    }

    public ReadOnlyObservableCollection(INotifyCollectionChanged source, IEnumerable<TSourceItem> items, Func<TSourceItem, TDestinationItem> converter)
        : base(new ObservableCollection<TDestinationItem>(items.Select(model => converter(model))))
    {
        this.source = source;
        this.converter = converter;
        this.source.CollectionChanged += OnSourceCollectionChanged;
        innerCollection = (ObservableCollection<TDestinationItem>)this.Items;
    }

    protected virtual TDestinationItem Convert(TSourceItem model)
    {
        return converter(model);
    }

    private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                for (int i = 0; i < e.NewItems!.Count; i++)
                {
                    innerCollection.Insert(e.NewStartingIndex + i, Convert((TSourceItem)e.NewItems[i]!));
                }
                break;

            case NotifyCollectionChangedAction.Move:
                if (e.OldItems!.Count == 1)
                {
                    innerCollection.Move(e.OldStartingIndex, e.NewStartingIndex);
                }
                else
                {
                    List<TDestinationItem> items = this.Skip(e.OldStartingIndex).Take(e.OldItems.Count).ToList();
                    for (int i = 0; i < e.OldItems.Count; i++)
                        innerCollection.RemoveAt(e.OldStartingIndex);

                    for (int i = 0; i < items.Count; i++)
                        innerCollection.Insert(e.NewStartingIndex + i, items[i]);
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                for (int i = 0; i < e.OldItems!.Count; i++)
                    innerCollection.RemoveAt(e.OldStartingIndex);
                break;

            case NotifyCollectionChangedAction.Replace:
                // remove
                for (int i = 0; i < e.OldItems!.Count; i++)
                    innerCollection.RemoveAt(e.OldStartingIndex);

                // add
                goto case NotifyCollectionChangedAction.Add;

            case NotifyCollectionChangedAction.Reset:
                innerCollection.Clear();
                for (int i = 0; i < e.NewItems!.Count; i++)
                    innerCollection.Add(Convert((TSourceItem)e.NewItems[i]!));
                break;

            default:
                break;
        }
    }
}
