namespace Kaigara.Menus
{
    public class MenuItemGroupDefinition : MenuItemDefinition
    {
        public MenuItemGroupDefinition(string name, string? label = null) 
            : base(name, label)
        {
        }

        internal override MenuItemViewModel Build()
            => new MenuItemGroupViewModel(this);
    }
}
