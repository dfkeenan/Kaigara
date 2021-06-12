using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kaigara.Collections.ObjectModel
{
    public static class ObservableCollectionExtensions
    {
        public static ObservableCollection<TDestinationItem> ToObservableCollectionOf<TDestinationItem, TSourceItem>(this ObservableCollection<TSourceItem> source, Func<TSourceItem, TDestinationItem> viewModelFactory)
        {
            return new ObservableCollection<TDestinationItem, TSourceItem>(source, viewModelFactory);
        }

        public static ObservableCollection<TDestinationItem> ToObservableCollectionOf<TDestinationItem, TSourceItem>(this ReadOnlyObservableCollection<TSourceItem> source, Func<TSourceItem, TDestinationItem> viewModelFactory)
        {
            return new ObservableCollection<TDestinationItem, TSourceItem>(source, source, viewModelFactory);
        }

        public static ReadOnlyObservableCollection<T> AsReadOnlyObservableCollection<T>(this ObservableCollection<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ReadOnlyObservableCollection<T>(source);
        }
    }

    public class ObservableCollection<TDestinationItem, TSourceItem> : ObservableCollection<TDestinationItem>
    {
        private readonly INotifyCollectionChanged source;
        private readonly Func<TSourceItem, TDestinationItem> converter;

        public ObservableCollection(ObservableCollection<TSourceItem> source, Func<TSourceItem, TDestinationItem> converter)
            : base(source.Select(model => converter(model)))
        {
            this.source = source;
            this.converter = converter;
            this.source.CollectionChanged += OnSourceCollectionChanged;

            
        }

        public ObservableCollection(INotifyCollectionChanged source, IEnumerable<TSourceItem> items, Func<TSourceItem, TDestinationItem> converter)
            : base(items.Select(model => converter(model)))
        {
            this.source = source;
            this.converter = converter;
            this.source.CollectionChanged += OnSourceCollectionChanged;
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
                        Insert(e.NewStartingIndex + i, Convert((TSourceItem)e.NewItems[i]!));
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldItems!.Count == 1)
                    {
                        Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else
                    {
                        List<TDestinationItem> items = this.Skip(e.OldStartingIndex).Take(e.OldItems.Count).ToList();
                        for (int i = 0; i < e.OldItems.Count; i++)
                            RemoveAt(e.OldStartingIndex);

                        for (int i = 0; i < items.Count; i++)
                            Insert(e.NewStartingIndex + i, items[i]);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems!.Count; i++)
                        RemoveAt(e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    // remove
                    for (int i = 0; i < e.OldItems!.Count; i++)
                        RemoveAt(e.OldStartingIndex);

                    // add
                    goto case NotifyCollectionChangedAction.Add;

                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    for (int i = 0; i < e.NewItems!.Count; i++)
                        Add(Convert((TSourceItem)e.NewItems[i]!));
                    break;

                default:
                    break;
            }
        }
    }
}
