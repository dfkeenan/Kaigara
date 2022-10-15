using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Markup.Xaml;
using Kaigara.Avalonia.ReactiveUI;
using Kaigara.Commands;
using Kaigara.MainWindow.ViewModels;
using ReactiveUI;

namespace Kaigara.MainWindow.Views;

public class MainWindowView : ReactiveChromeWindow<MainWindowViewModel>
{
    public MainWindowView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            ViewModel?.CommandManager.SyncKeyBindings(this.KeyBindings).DisposeWith(d);
            if(ViewModel != null)
            {
                ViewModel.View = this;
            }
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
#if DEBUG
        this.AttachDevTools();
#endif
    }
}
