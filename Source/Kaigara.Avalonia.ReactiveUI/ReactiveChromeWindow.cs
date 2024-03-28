using Kaigara.Avalonia.Controls;
using ReactiveUI;

namespace Kaigara.Avalonia.ReactiveUI;

/// <summary>
/// A ReactiveUI Window that implements <see cref="IViewFor{TViewModel}"/>
/// and will activate your ViewModel automatically if it supports activation.
/// </summary>
/// <typeparam name="TViewModel">ViewModel type.</typeparam>
public class ReactiveChromeWindow<TViewModel> : ChromeWindow, IViewFor<TViewModel> where TViewModel : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveChromeWindow{TViewModel}"/> class.
    /// </summary>
    public ReactiveChromeWindow()
    {
        // This WhenActivated block calls ViewModel's WhenActivated
        // block if the ViewModel implements IActivatableViewModel.
        this.WhenActivated(disposables => { });
    }

    /// <summary>
    /// The ViewModel.
    /// </summary>
    public TViewModel? ViewModel
    {
        get => DataContext as TViewModel;
        set => DataContext = value;
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TViewModel?)value;
    }
}
