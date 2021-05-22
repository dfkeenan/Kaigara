using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kaigara.Menus.Controls
{
    using MenuViewModel = MenuViewModel;
    public partial class DynamicMenu : UserControl
    {
        public static readonly DirectProperty<DynamicMenu, MenuViewModel?> MenuProperty =
            AvaloniaProperty.RegisterDirect<DynamicMenu, MenuViewModel?>(
                nameof(Menu),
                o => o.Menu,
                (o, v) => o.Menu = v);

        private MenuViewModel? menu = null;

        public MenuViewModel? Menu
        {
            get { return menu; }
            set { SetAndRaise(MenuProperty, ref menu, value); }
        }

        public DynamicMenu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
