using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kaigara.Shell.Views;

public class ShellView : UserControl
{
    public ShellView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
