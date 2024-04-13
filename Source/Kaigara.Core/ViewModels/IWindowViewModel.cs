using Avalonia.Controls;

namespace Kaigara.ViewModels;

public interface IWindowViewModel
{
    double Height { get; set; }
    Uri? IconUri { get; set; }
    WindowStartupLocation StartupLocation { get; set; }
    string? Title { get; set; }
    double Width { get; set; }
    bool CanResize { get; set; }
    double MinWidth { get; set; }
    double MinHeight { get; set; }
}
