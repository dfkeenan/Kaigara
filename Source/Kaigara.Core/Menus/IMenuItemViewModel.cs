using System.Collections.Generic;
using System.Windows.Input;
using Avalonia.Input;

namespace Kaigara.Menus
{
    public interface IMenuItemViewModel
    {
        ICommand? Command { get; }
        object? CommandParameter { get; }
        KeyGesture? InputGesture { get; }
        bool IsVisible { get; }
        IEnumerable<IMenuItemViewModel> Items { get; }
        string? Label { get; }
        string? IconName { get; }
        void Dispose();
    }

    public interface IMenuItemGroupViewModel
    {

    }
}