namespace Kaigara.Menus;

[AttributeUsage(AttributeTargets.Class)]
public class MenuItemDefinitionAttribute : Attribute
{
    public MenuItemDefinitionAttribute(string name, string path)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
        }

        Name = name;

        Location = new MenuItemLocation(path);
    }

    public string Name { get; }
    public string? IconName { get; set; }
    public string? Label { get; set; }
    public int DisplayOrder { get; set; }

    public MenuItemLocation Location { get; }
}
