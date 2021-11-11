using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ExampleApplication.Tools.Views;

public partial class RightToolView : UserControl
{
    public RightToolView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
