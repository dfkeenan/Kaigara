using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Kaigara.Services;
public interface IRequireStorageProvider
{
    IStorageProvider? StorageProvider { get; set; }
}

public static class StorageProviderExtensions
{
    public static IDisposable BindStorageProvider(this Visual visual, IRequireStorageProvider requireStorageProvider)
    {
        var storageProviderProxy = TopLevel.GetTopLevel(visual)?.StorageProvider!;
        requireStorageProvider.StorageProvider = storageProviderProxy;
        return Disposable.Create(() => requireStorageProvider.StorageProvider = null);
    }

    public static IDisposable? TryBindStorageProvider(this Visual visual, object? viewModel)
    {
        if (viewModel is IRequireStorageProvider storageProvider)
        {
            return BindStorageProvider(visual, storageProvider);
        }

        return null;
    }
}

