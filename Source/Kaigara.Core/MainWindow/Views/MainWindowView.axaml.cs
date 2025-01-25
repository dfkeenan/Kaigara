using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kaigara.Avalonia.ReactiveUI;
using Kaigara.Commands;
using Kaigara.MainWindow.ViewModels;
using Kaigara.Services;
using ReactiveUI;

namespace Kaigara.MainWindow.Views;

public class MainWindowView : ReactiveChromeWindow<MainWindowViewModel>
{
    public MainWindowView()
    {
        InitializeComponent();

        if (Design.IsDesignMode) return;

        this.WhenActivated(d =>
        {
            ViewModel?.CommandManager.SyncKeyBindings(this.KeyBindings).DisposeWith(d);

            if (ViewModel?.Shell is IRequireStorageProvider shell)
            {
                this.BindStorageProvider(shell).DisposeWith(d);
            }
        });

    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
#if DEBUG
        //this.AttachDevTools();
#endif
    }
}
