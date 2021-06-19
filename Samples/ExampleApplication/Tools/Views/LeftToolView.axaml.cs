using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ExampleApplication.Tools.Views
{
    public partial class LeftToolView : UserControl
    {
        public LeftToolView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
