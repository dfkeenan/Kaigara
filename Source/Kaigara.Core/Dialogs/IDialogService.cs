using Avalonia.Controls;

namespace Kaigara.Dialogs;
public interface IDialogService
{
    Task<string[]?> ShowAsync(OpenFileDialog openFileDialog);
    Task<string?> ShowAsync(SaveFileDialog saveFileDialog);
}