using System.Windows.Input;
using Avalonia.Input;
using Kaigara.Commands;

namespace Kaigara.Menus;

public interface IMenuItemViewModel : IDisposable
{
    ICommand? Command { get; }
    CanExecuteBehavior CanExecuteBehavior { get; }
    KeyGesture? InputGesture { get; }
    bool IsVisible { get; }
    IEnumerable<IMenuItemViewModel> Items { get; }
    string? Label { get; }
    string? IconName { get; }
}
