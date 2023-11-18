using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Kaigara.Services;
public interface IRequireStorageProvider
{
    IStorageProvider? StorageProvider { get; set; }
}

internal class StorageProviderProxy(Visual visual, IRequireStorageProvider requireStorageProvider) : IStorageProvider, IDisposable
{
    private readonly Visual visual = visual ?? throw new ArgumentNullException(nameof(visual));
    private readonly IRequireStorageProvider requireStorageProvider = requireStorageProvider ?? throw new ArgumentNullException(nameof(requireStorageProvider));

    public bool CanOpen => StorageProvider.CanOpen is true;

    public bool CanSave => StorageProvider.CanSave is true;

    public bool CanPickFolder => StorageProvider.CanPickFolder is true;

    private IStorageProvider StorageProvider => TopLevel.GetTopLevel(visual)?.StorageProvider!;

    public void Dispose()
    {
        requireStorageProvider.StorageProvider = null;
    }

    public Task<IStorageBookmarkFile?> OpenFileBookmarkAsync(string bookmark)
    {
        return StorageProvider.OpenFileBookmarkAsync(bookmark);
    }

    public Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
    {
        return StorageProvider.OpenFilePickerAsync(options);
    }

    public Task<IStorageBookmarkFolder?> OpenFolderBookmarkAsync(string bookmark)
    {
        return StorageProvider.OpenFolderBookmarkAsync(bookmark);
    }

    public Task<IReadOnlyList<IStorageFolder>> OpenFolderPickerAsync(FolderPickerOpenOptions options)
    {
        return StorageProvider.OpenFolderPickerAsync(options);
    }

    public Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options)
    {
        return StorageProvider.SaveFilePickerAsync(options);
    }

    public Task<IStorageFile?> TryGetFileFromPathAsync(Uri filePath)
    {
        return StorageProvider.TryGetFileFromPathAsync(filePath);
    }

    public Task<IStorageFolder?> TryGetFolderFromPathAsync(Uri folderPath)
    {
        return StorageProvider.TryGetFolderFromPathAsync(folderPath);
    }

    public Task<IStorageFolder?> TryGetWellKnownFolderAsync(WellKnownFolder wellKnownFolder)
    {
        return StorageProvider.TryGetWellKnownFolderAsync(wellKnownFolder);
    }
}

public static class StorageProviderExtensions
{
    public static IDisposable BindStorageProvider(this Visual visual, IRequireStorageProvider storageProvider)
    {
        var storageProviderProxy = new StorageProviderProxy(visual, storageProvider);
        storageProvider.StorageProvider = storageProviderProxy;
        return storageProviderProxy;
    }

    public static IDisposable? TryBindStorageProvider(this Visual visual, object? viewModel)
    {
        if (viewModel is IRequireStorageProvider storageProvider)
        {
            var storageProviderProxy = new StorageProviderProxy(visual, storageProvider);
            storageProvider.StorageProvider = storageProviderProxy;
            return storageProviderProxy; 
        }

        return null;
    }
}

