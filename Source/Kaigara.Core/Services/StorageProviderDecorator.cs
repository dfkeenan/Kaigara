using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Kaigara.MainWindow.Views;
using ReactiveUI;

namespace Kaigara.Services;

internal class StorageProviderDecorator<T> : IStorageProvider
    where T : TopLevel
{
    private IStorageProvider? storageProvider = default!;

    public StorageProviderDecorator()
    {
        MessageBus.Current.Listen<ViewCreated<T>>()
            .Take(1)
            .Subscribe(m =>
            {
                storageProvider = m.View.StorageProvider;
            });
    }

    public bool CanOpen => storageProvider?.CanOpen ?? false;

    public bool CanSave => storageProvider?.CanSave ?? false;

    public bool CanPickFolder => storageProvider?.CanPickFolder ?? false;

    public Task<IStorageBookmarkFile?> OpenFileBookmarkAsync(string bookmark)
    {
        if (storageProvider is null) throw new InvalidOperationException("${nameof(StorageProvider)} is null");
        return storageProvider.OpenFileBookmarkAsync(bookmark);
    }

    public Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
    {
        if (storageProvider is null) throw new InvalidOperationException("${nameof(StorageProvider)} is null");
        return storageProvider.OpenFilePickerAsync(options);
    }

    public Task<IStorageBookmarkFolder?> OpenFolderBookmarkAsync(string bookmark)
    {
        if (storageProvider is null) throw new InvalidOperationException("${nameof(StorageProvider)} is null");
        return storageProvider.OpenFolderBookmarkAsync(bookmark);
    }

    public Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options)
    {
        if (storageProvider is null) throw new InvalidOperationException("${nameof(StorageProvider)} is null");
        return storageProvider.OpenFolderPickerAsync(options);
    }

    public Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options)
    {
        if (storageProvider is null) throw new InvalidOperationException("${nameof(StorageProvider)} is null");
        return storageProvider.SaveFilePickerAsync(options);
    }

    public Task<IStorageFile?> TryGetFileFromPathAsync(Uri filePath)
    {
        if (storageProvider is null) throw new InvalidOperationException("${nameof(StorageProvider)} is null");
        return storageProvider.TryGetFileFromPathAsync(filePath);
    }

    public Task<IStorageFolder?> TryGetFolderFromPathAsync(Uri folderPath)
    {
        if (storageProvider is null) throw new InvalidOperationException("${nameof(StorageProvider)} is null");
        return storageProvider.TryGetFolderFromPathAsync(folderPath);
    }

    public Task<IStorageFolder?> TryGetWellKnownFolderAsync(WellKnownFolder wellKnownFolder)
    {
        if (storageProvider is null) throw new InvalidOperationException("${nameof(StorageProvider)} is null");
        return storageProvider.TryGetWellKnownFolderAsync(wellKnownFolder);
    }
}
