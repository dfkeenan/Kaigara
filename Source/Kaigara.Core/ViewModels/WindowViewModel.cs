using Avalonia.Controls;
using ReactiveUI;
//using AutoNotify;

namespace Kaigara.ViewModels;

//[AutoNotifyAll]
public abstract partial class WindowViewModel : ActivatableViewModel, IWindowViewModel
{
    protected WindowViewModel()
    {
        Width = double.NaN;
        Height = double.NaN;
        MinWidth = double.NaN;
        MinHeight = double.NaN;
    }

    private string? title;

    public string? Title
    {
        get { return title; }
        set { this.RaiseAndSetIfChanged(ref title, value); }
    }

    private Uri? iconUri;

    public Uri? IconUri
    {
        get { return iconUri; }
        set { this.RaiseAndSetIfChanged(ref iconUri, value); }
    }


    private WindowStartupLocation startupLocation;

    public WindowStartupLocation StartupLocation
    {
        get { return startupLocation; }
        set { this.RaiseAndSetIfChanged(ref startupLocation, value); }
    }

    private double width;

    public double Width
    {
        get { return width; }
        set { this.RaiseAndSetIfChanged(ref width, value); }
    }

    private double height;

    public double Height
    {
        get { return height; }
        set { this.RaiseAndSetIfChanged(ref height, value); }
    }


    private double minWidth;

    public double MinWidth
    {
        get { return minWidth; }
        set { this.RaiseAndSetIfChanged(ref minWidth, value); }
    }

    private double minHeight;

    public double MinHeight
    {
        get { return minHeight; }
        set { this.RaiseAndSetIfChanged(ref minHeight, value); }
    }

    private bool canResize = true;

    public bool CanResize
    {
        get { return canResize; }
        set { this.RaiseAndSetIfChanged(ref canResize, value); }
    }
}
