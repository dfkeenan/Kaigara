using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Kaigara.MainWindow.ViewModels;

namespace Kaigara.Dialogs;
public class DialogService : IDialogService
{    
    private readonly IComponentContext context;
    private Window? window;

    protected Window Window
        => LazyInitializer.EnsureInitialized(ref window, () => context.Resolve<MainWindowViewModel>().View);


    public DialogService(IComponentContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<IStorageFile?> ShowAsync(FilePickerSaveOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        return Window.StorageProvider.SaveFilePickerAsync(options);
    }

    public Task<IReadOnlyList<IStorageFile>> ShowAsync(FilePickerOpenOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        return Window.StorageProvider.OpenFilePickerAsync(options);
    }
}
