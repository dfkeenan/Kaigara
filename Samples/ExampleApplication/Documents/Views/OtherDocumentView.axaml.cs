using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ExampleApplication.Documents.Views;

public partial class OtherDocumentView : UserControl
{
    public OtherDocumentView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
