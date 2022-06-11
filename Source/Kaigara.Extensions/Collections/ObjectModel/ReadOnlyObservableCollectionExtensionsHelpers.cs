using System.Collections.ObjectModel;

namespace Kaigara.Collections.ObjectModel;

public static class ReadOnlyObservableCollectionExtensionsHelpers
{

    public static ReadOnlyObservableCollection<T> AsReadOnlyObservableCollection<T>(this ObservableCollection<T> source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return new ReadOnlyObservableCollection<T>(source);
    }
    public static ReadOnlyObservableCollection<TDestinationItem> ToReadOnlyObservableCollectionOf<TSourceItem, TDestinationItem>(this ObservableCollection<TSourceItem> source, Func<TSourceItem, TDestinationItem> viewModelFactory)
    {
        return new ReadOnlyObservableCollection<TSourceItem, TDestinationItem>(source, viewModelFactory);
    }

    public static ReadOnlyObservableCollection<TDestinationItem> ToReadOnlyObservableCollectionOf<TSourceItem, TDestinationItem>(this ReadOnlyObservableCollection<TSourceItem> source, Func<TSourceItem, TDestinationItem> viewModelFactory)
    {
        return new ReadOnlyObservableCollection<TSourceItem, TDestinationItem>(source, source, viewModelFactory);
    }
}