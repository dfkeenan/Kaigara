using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kaigara.Avalonia.Controls;
using Kaigara.Avalonia.ReactiveUI;
using Kaigara.Dialogs.ViewModels;
using Kaigara.MainWindow.ViewModels;

namespace Kaigara.Dialogs.Views;
public partial class DialogView : ReactiveChromeWindow<DialogViewModel>
{
    public DialogView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
