using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ExampleApplication.Documents.Views
{
    public partial class ExampleDocumentView : UserControl
    {
        public ExampleDocumentView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
