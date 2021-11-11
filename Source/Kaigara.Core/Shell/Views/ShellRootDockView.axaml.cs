using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kaigara.Shell.Views
{
    public class ShellRootDockView : UserControl
    {
        public ShellRootDockView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
