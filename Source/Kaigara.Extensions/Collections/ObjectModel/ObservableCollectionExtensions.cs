using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Kaigara.Collections.ObjectModel;

public static class ObservableCollectionExtensions
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

    public static void MapSourceCollectionChanged<TSourceItem, TDestinationItem>(this IList<TDestinationItem> destination, NotifyCollectionChangedEventArgs e, Func<TSourceItem, TDestinationItem> map)
    {
        if (destination is null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (map is null)
        {
            throw new ArgumentNullException(nameof(map));
        }

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                for (int i = 0; i < e.NewItems!.Count; i++)
                {
                    destination.Insert(e.NewStartingIndex + i, map((TSourceItem)e.NewItems[i]!));
                }
                break;

            case NotifyCollectionChangedAction.Move:
                if (e.OldItems!.Count == 1)
                {
                    Move(e.OldStartingIndex, e.NewStartingIndex);
                }
                else
                {
                    List<TDestinationItem> items = destination.Skip(e.OldStartingIndex).Take(e.OldItems.Count).ToList();
                    for (int i = 0; i < e.OldItems.Count; i++)
                        destination.RemoveAt(e.OldStartingIndex);

                    for (int i = 0; i < items.Count; i++)
                        destination.Insert(e.NewStartingIndex + i, items[i]);
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                for (int i = 0; i < e.OldItems!.Count; i++)
                    destination.RemoveAt(e.OldStartingIndex);
                break;

            case NotifyCollectionChangedAction.Replace:
                // remove
                for (int i = 0; i < e.OldItems!.Count; i++)
                    destination.RemoveAt(e.OldStartingIndex);

                // add
                goto case NotifyCollectionChangedAction.Add;

            case NotifyCollectionChangedAction.Reset:
                destination.Clear();
                for (int i = 0; i < e.NewItems!.Count; i++)
                    destination.Add(map((TSourceItem)e.NewItems[i]!));
                break;

            default:
                break;
        }

        void Move(int oldIndex, int newIndex)
        {

            TDestinationItem removedItem = destination[oldIndex];

            destination.RemoveAt(oldIndex);
            destination.Insert(newIndex, removedItem);
        }
    }

    public static IDisposable SyncTo<T>(this INotifyCollectionChanged source, IList<T> destination, bool force = false)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (destination is null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (!(source is IEnumerable<T> sourceEnumerable))
        {
            throw new ArgumentException($"Must be of type {typeof(IEnumerable<T>).FullName}", nameof(source));
        }

        if (force || !destination.Any())
        {
            destination.Clear();
            foreach (var item in sourceEnumerable)
            {
                destination.Add(item);
            }
        }
        else if (!destination.SequenceEqual(sourceEnumerable))
        {
            throw new ArgumentException($"Must be empty or have a sequence equal to source", nameof(destination));
        }


        source.CollectionChanged += OnSourceCollectionChanged;

        return Disposable.Create(() => source.CollectionChanged -= OnSourceCollectionChanged);


        void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems!.Count; i++)
                    {
                        destination.Insert(e.NewStartingIndex + i, (T)e.NewItems[i]!);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldItems!.Count == 1)
                    {
                        Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else
                    {
                        List<T> items = destination.Skip(e.OldStartingIndex).Take(e.OldItems.Count).ToList();
                        for (int i = 0; i < e.OldItems.Count; i++)
                            destination.RemoveAt(e.OldStartingIndex);

                        for (int i = 0; i < items.Count; i++)
                            destination.Insert(e.NewStartingIndex + i, items[i]);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems!.Count; i++)
                        destination.RemoveAt(e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    // remove
                    for (int i = 0; i < e.OldItems!.Count; i++)
                        destination.RemoveAt(e.OldStartingIndex);

                    // add
                    goto case NotifyCollectionChangedAction.Add;

                case NotifyCollectionChangedAction.Reset:
                    destination.Clear();
                    for (int i = 0; i < e.NewItems!.Count; i++)
                        destination.Add((T)e.NewItems[i]!);
                    break;

                default:
                    break;
            }
        }

        void Move(int oldIndex, int newIndex)
        {

            T removedItem = destination[oldIndex];

            destination.RemoveAt(oldIndex);
            destination.Insert(newIndex, removedItem);
        }
    }

    public static IDisposable SyncItemsTo<T>(this INotifyCollectionChanged source, ICollection<T> destination)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (destination is null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (!(source is IEnumerable<T> sourceEnumerable))
        {
            throw new ArgumentException($"Must be of type {typeof(IEnumerable<T>).FullName}", nameof(source));
        }

        foreach (var item in sourceEnumerable)
        {
            destination.Add(item);
        }


        source.CollectionChanged += OnSourceCollectionChanged;

        return Disposable.Create(() =>
        {
            source.CollectionChanged -= OnSourceCollectionChanged;
            foreach (var item in sourceEnumerable)
            {
                destination.Remove(item);
            }
        });


        void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems!)
                    {
                        destination.Add((T)item);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems!)
                    {
                        destination.Remove((T)item);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    // remove
                    foreach (var item in e.OldItems!)
                    {
                        destination.Remove((T)item);
                    }

                    // add
                    goto case NotifyCollectionChangedAction.Add;

                case NotifyCollectionChangedAction.Reset:
                    //TODO: What
                    break;

                default:
                    break;
            }
        }
    }

    private class Disposable : IDisposable
    {
        private readonly Action dispose;

        public Disposable(Action dispose)
        {
            this.dispose = dispose;
        }

        public static IDisposable Create(Action dispose)
        {
            if (dispose is null)
            {
                throw new ArgumentNullException(nameof(dispose));
            }

            return new Disposable(dispose);
        }

        public void Dispose()
        {
            dispose?.Invoke();
        }
    }
}