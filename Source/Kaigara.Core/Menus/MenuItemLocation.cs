namespace Kaigara.Menus;

public class MenuItemLocation : UIComponentLocation
{
    public MenuItemLocation(string path) : base(path)
    {
    }

    public MenuItemLocation(IEnumerable<string> path) : base(path)
    {
    }

    public MenuItemLocation GetSubMenuPath(string name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        return new MenuItemLocation(PathSegments.Append(name));
    }
}
