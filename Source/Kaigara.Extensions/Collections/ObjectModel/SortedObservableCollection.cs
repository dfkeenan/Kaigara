using System.Collections.ObjectModel;

namespace Kaigara.Collections.ObjectModel;

public class SortedObservableCollection<T> : ObservableCollection<T>
{
    private readonly IComparer<T> comparer;
    private readonly List<T> items;

    public SortedObservableCollection(IComparer<T>? comparer = null)
        : base(new List<T>())
    {
        this.items = (List<T>)Items;
        this.comparer = comparer ?? Comparer<T>.Default;
    }

    protected override void InsertItem(int index, T item)
    {
        var i = items.BinarySearch(item, comparer);
        index = i < 0 ? ~i : i;
        base.InsertItem(index, item);
    }
}
