using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kaigara.Avalonia.ReactiveUI;
using Kaigara.Commands;
using Kaigara.MainWindow.ViewModels;
using ReactiveUI;

namespace Kaigara.MainWindow.Views;

public record class ViewCreated<T>(T View) where T : TopLevel;

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

        MessageBus.Current.SendMessage(new ViewCreated<MainWindowView>(this));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
#if DEBUG
        this.AttachDevTools();
#endif
    }
}
