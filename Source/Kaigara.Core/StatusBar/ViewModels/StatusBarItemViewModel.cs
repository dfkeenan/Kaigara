using Avalonia.Controls;
using Kaigara.ViewModels;

namespace Kaigara.StatusBar.ViewModels;
public class StatusBarItemViewModel : ViewModel
{
    public StatusBarItemViewModel(int index, GridLength width)
    {
        Index = index;
        Width = width;
    }

    public int Index { get; }

    public GridLength Width { get; }
}

internal class StatusBarItemViewModelComparer : IComparer<StatusBarItemViewModel>
{
    public int Compare(StatusBarItemViewModel? x, StatusBarItemViewModel? y)
    {
        return Comparer<int?>.Default.Compare(x?.Index, y?.Index);
    }
}
