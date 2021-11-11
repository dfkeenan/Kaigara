using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ExampleApplication.Tools.Views;

public partial class BottomToolView : UserControl
{
    public BottomToolView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
