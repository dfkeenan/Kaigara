using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Templates;
using Avalonia.Layout;

namespace Kaigara.Avalonia.Controls
{
    public class ToolBarTray : ItemsControl
    {
        private static readonly FuncTemplate<IPanel> DefaultPanel =
            new FuncTemplate<IPanel>(() => new WrapPanel { Orientation = Orientation.Horizontal });

        static ToolBarTray()
        {
            ItemsPanelProperty.OverrideDefaultValue<ToolBarTray>(DefaultPanel);
        }

        public ToolBarTray()
        {
            
        }

        protected override IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator<ToolBar>(this, ContentControl.ContentProperty,
                ContentControl.ContentTemplateProperty);
        }
    }
}
