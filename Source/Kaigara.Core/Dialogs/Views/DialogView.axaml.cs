using System.Reactive;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kaigara.Avalonia.ReactiveUI;
using Kaigara.Dialogs.ViewModels;
using Kaigara.Services;
using ReactiveUI;

namespace Kaigara.Dialogs.Views;
public partial class DialogView : ReactiveChromeWindow<DialogViewModel>
{
    public DialogView()
    {
        InitializeComponent();
        if (Design.IsDesignMode) return;
        this.WhenActivated(d =>
        {
            this.TryBindStorageProvider(ViewModel)?.DisposeWith(d);

            ViewModel?.CloseInteraction.RegisterHandler(c =>
            {
                this.Close(c.Input);
                c.SetOutput(Unit.Default);
            }).DisposeWith(d);
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
