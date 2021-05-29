using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kaigara.ViewModels
{
    public static class ObservableViewModelCollectionExtensions
    {
        public static ObservableCollection<TViewModel> ToObservableViewModelCollection<TViewModel, TModel>(this ObservableCollection<TModel> source, Func<TModel, TViewModel> viewModelFactory)
        {
            return new ObservableViewModelCollection<TViewModel, TModel>(source, viewModelFactory);
        }

        public static ObservableCollection<TViewModel> ToObservableViewModelCollection<TViewModel, TModel>(this ReadOnlyObservableCollection<TModel> source, Func<TModel, TViewModel> viewModelFactory)
        {
            return new ObservableViewModelCollection<TViewModel, TModel>(source, source, viewModelFactory);
        }
    }

    public class ObservableViewModelCollection<TViewModel, TModel> : ObservableCollection<TViewModel>
    {
        private readonly INotifyCollectionChanged source;
        private readonly Func<TModel, TViewModel> viewModelFactory;

        public ObservableViewModelCollection(ObservableCollection<TModel> source, Func<TModel, TViewModel> viewModelFactory)
            : base(source.Select(model => viewModelFactory(model)))
        {
            this.source = source;
            this.viewModelFactory = viewModelFactory;
            this.source.CollectionChanged += OnSourceCollectionChanged;
        }

        public ObservableViewModelCollection(INotifyCollectionChanged source, IEnumerable<TModel> items, Func<TModel, TViewModel> viewModelFactory)
            : base(items.Select(model => viewModelFactory(model)))
        {
            this.source = source;
            this.viewModelFactory = viewModelFactory;
            this.source.CollectionChanged += OnSourceCollectionChanged;
        }

        protected virtual TViewModel CreateViewModel(TModel model)
        {
            return viewModelFactory(model);
        }

        private void OnSourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (int i = 0; i < e.NewItems!.Count; i++)
                    {
                        this.Insert(e.NewStartingIndex + i, CreateViewModel((TModel)e.NewItems[i]!));
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldItems!.Count == 1)
                    {
                        this.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else
                    {
                        List<TViewModel> items = this.Skip(e.OldStartingIndex).Take(e.OldItems.Count).ToList();
                        for (int i = 0; i < e.OldItems.Count; i++)
                            this.RemoveAt(e.OldStartingIndex);

                        for (int i = 0; i < items.Count; i++)
                            this.Insert(e.NewStartingIndex + i, items[i]);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    for (int i = 0; i < e.OldItems!.Count; i++)
                        this.RemoveAt(e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    // remove
                    for (int i = 0; i < e.OldItems!.Count; i++)
                        this.RemoveAt(e.OldStartingIndex);

                    // add
                    goto case NotifyCollectionChangedAction.Add;

                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    for (int i = 0; i < e.NewItems!.Count; i++)
                        this.Add(CreateViewModel((TModel)e.NewItems[i]!));
                    break;

                default:
                    break;
            }
        }
    }
}
