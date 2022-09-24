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
        innerCollection.MapSourceCollectionChanged(e, converter);
    }
}
