using System.Windows.Input;
using Avalonia.Input;

namespace Kaigara.ToolBars
{
    public interface IToolBarItemViewModel : IDisposable
    {
        ICommand? Command { get; }
        object? CommandParameter { get; }
        KeyGesture? InputGesture { get; }
        bool IsVisible { get; }
        string? Label { get; }
        string? IconName { get; }
    }
}