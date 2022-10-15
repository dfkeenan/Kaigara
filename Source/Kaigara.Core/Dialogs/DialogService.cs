using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls;
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

    public Task<string?> ShowAsync(SaveFileDialog saveFileDialog)
    {
        if (saveFileDialog is null)
        {
            throw new ArgumentNullException(nameof(saveFileDialog));
        }

        return saveFileDialog.ShowAsync(Window);
    }

    public Task<string[]?> ShowAsync(OpenFileDialog openFileDialog)
    {
        if (openFileDialog is null)
        {
            throw new ArgumentNullException(nameof(openFileDialog));
        }

        return openFileDialog.ShowAsync(Window);
    }
}
