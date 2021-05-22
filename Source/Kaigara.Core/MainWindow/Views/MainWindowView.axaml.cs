using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kaigara.Avalonia.ReactiveUI;
using Kaigara.MainWindow.ViewModels;

namespace Kaigara.MainWindow.Views
{
    public class MainWindowView : ReactiveChromeWindow<MainWindowViewModel>
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
