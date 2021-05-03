using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Kaigara.Shell.ViewModels;

namespace Kaigara.Shell.Views
{
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
}
