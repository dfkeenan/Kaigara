using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kaigara.Avalonia.Controls;
using Kaigara.Avalonia.ReactiveUI;
using Kaigara.Dialogs.ViewModels;
using Kaigara.MainWindow.ViewModels;
using Kaigara.Services;
using Kaigara.Shell;
using ReactiveUI;

namespace Kaigara.Dialogs.Views;
public partial class DialogView : ReactiveChromeWindow<DialogViewModel>
{
    public DialogView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            this.TryBindStorageProvider(ViewModel)?.DisposeWith(d);
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
