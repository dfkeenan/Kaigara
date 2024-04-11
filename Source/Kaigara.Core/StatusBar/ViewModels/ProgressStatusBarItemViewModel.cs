using Avalonia.Controls;
using ReactiveUI;

namespace Kaigara.StatusBar.ViewModels;
public class ProgressStatusBarItemViewModel : StatusBarItemViewModel
{
    public ProgressStatusBarItemViewModel()
        : base(1, GridLength.Auto)
    {
    }

    private double? progress = null;

    public double? Progress
    {
        get => progress;
        set
        {
            this.RaiseAndSetIfChanged(ref progress, value);
        }
    }
}
