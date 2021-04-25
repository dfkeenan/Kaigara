using Avalonia;
using Avalonia.VisualTree;
using Avalonia.Controls;
using ReactiveUI;
using Kaigara.Avalonia.Controls;

namespace Kaigara.Avalonia.ReactiveUI
{
    /// <summary>
    /// A ReactiveUI Window that implements <see cref="IViewFor{TViewModel}"/>
    /// and will activate your ViewModel automatically if it supports activation.
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel type.</typeparam>
    public class ReactiveChromeWindow<TViewModel> : ChromeWindow, IViewFor<TViewModel> where TViewModel : class
    {
        public static readonly StyledProperty<TViewModel?> ViewModelProperty = AvaloniaProperty
            .Register<ReactiveChromeWindow<TViewModel>, TViewModel?>(nameof(ViewModel));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveChromeWindow{TViewModel}"/> class.
        /// </summary>
        public ReactiveChromeWindow()
        {
            DataContextChanged += (sender, args) => ViewModel = DataContext as TViewModel;
        }

        /// <summary>
        /// The ViewModel.
        /// </summary>
        public TViewModel? ViewModel
        {
            get => GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel?)value;
        }
    }
}
