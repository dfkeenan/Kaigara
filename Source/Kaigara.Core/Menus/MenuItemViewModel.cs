using System.Windows.Input;
using Avalonia.Input;
using ReactiveUI;

namespace Kaigara.Menus
{
    public class MenuItemViewModel : ReactiveObject, IMenuItemViewModel, IDisposable
    {
        private ICommand? command;
        private object? commandParameter;
        private KeyGesture? inputGesture;
        private bool isVisible;
        private string label;
        private string? iconName;

        public MenuItemViewModel(string label)
        {
            this.label = label ?? throw new ArgumentNullException(nameof(label));
        }

        public string Label { get => label; set => this.RaiseAndSetIfChanged(ref label, value); }
        public string? IconName { get => iconName; set => this.RaiseAndSetIfChanged(ref iconName, value); }

        public ICommand? Command { get => command; set => this.RaiseAndSetIfChanged(ref command, value); }

        public object? CommandParameter { get => commandParameter; set => this.RaiseAndSetIfChanged(ref commandParameter, value); }

        public KeyGesture? InputGesture { get => inputGesture; set => this.RaiseAndSetIfChanged(ref inputGesture, value); }

        public bool IsVisible { get => isVisible; set => this.RaiseAndSetIfChanged(ref isVisible, value); }

        public IList<IMenuItemViewModel> Items { get; } = new List<IMenuItemViewModel>();

        IEnumerable<IMenuItemViewModel> IMenuItemViewModel.Items => this.Items;

        public void Dispose()
        {

        }
    }
}
