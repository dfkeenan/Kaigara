using Avalonia.Threading;
using Kaigara.Collections.ObjectModel;
using Kaigara.ViewModels;

namespace Kaigara.StatusBar.ViewModels;
public class StatusBarViewModel : ActivatableViewModel, IStatusBar
{
    private readonly MessageStatusBarItemViewModel messageStatusBarItemViewModel;
    private readonly ProgressStatusBarItemViewModel progressStatusBarItemViewModel;

    public StatusBarViewModel()
    {
        messageStatusBarItemViewModel = new MessageStatusBarItemViewModel();
        progressStatusBarItemViewModel = new ProgressStatusBarItemViewModel();

        Items = new SortedObservableCollection<StatusBarItemViewModel>(new StatusBarItemViewModelComparer())
        {
            messageStatusBarItemViewModel,
            progressStatusBarItemViewModel
        };
    }

    public SortedObservableCollection<StatusBarItemViewModel> Items { get; }

    public string? Status
    {
        get => messageStatusBarItemViewModel.Status;
        private set => messageStatusBarItemViewModel.Status = value;
    }

    public double? Progress
    {
        get => progressStatusBarItemViewModel.Progress;
        private set
        {
            progressStatusBarItemViewModel.Progress = value;
        }
    }

    public void UpdateStatus(string? status, double? progress = null)
    {
        Dispatcher.UIThread.Post(() =>
        {
            Status = status;
            Progress = progress;
        });
    }

    public void Ready()
    {
        UpdateStatus(null, null);
    }
}
