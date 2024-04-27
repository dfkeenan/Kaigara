using System.Windows.Input;
using Avalonia.Input;
using Kaigara.Commands;

namespace Kaigara.Toolbars;

public interface IToolbarItemViewModel : IDisposable
{
    ICommand? Command { get; }
    CanExecuteBehavior CanExecuteBehavior { get; }
    KeyGesture? InputGesture { get; }
    bool IsVisible { get; }
    string? Label { get; }
    string? IconName { get; }
}
