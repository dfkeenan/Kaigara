namespace Kaigara.Menus
{
    internal class DefinedMenuItemGroupViewModel : DefinedMenuItemViewModel, IMenuItemGroupViewModel
    {
        public DefinedMenuItemGroupViewModel(MenuItemGroupDefinition definition) 
            : base(definition)
        {
        }
    }
}
