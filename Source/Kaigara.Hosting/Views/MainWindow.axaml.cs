using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kaigara.Avalonia.ReactiveUI;
using Kaigara.ViewModels;

namespace Kaigara.Views
{
    public class MainWindow : ReactiveChromeWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
