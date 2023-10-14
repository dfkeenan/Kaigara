using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Kaigara.Dialogs;
public interface IDialogService
{
    public Task<IStorageFile?> ShowAsync(FilePickerSaveOptions options);

    public Task<IReadOnlyList<IStorageFile>> ShowAsync(FilePickerOpenOptions options);
}