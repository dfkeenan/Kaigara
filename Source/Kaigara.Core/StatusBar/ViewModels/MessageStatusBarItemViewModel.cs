using Avalonia.Controls;
using ReactiveUI;

namespace Kaigara.StatusBar.ViewModels;
public class MessageStatusBarItemViewModel : StatusBarItemViewModel
{
    public MessageStatusBarItemViewModel()
        : base(0, GridLength.Star)
    {
    }

    public string? DefaultStatus { get; set; } = "Ready";

    private string? status;

    public string? Status
    {
        get => status ?? DefaultStatus;
        set => this.RaiseAndSetIfChanged(ref status, value);
    }
}
