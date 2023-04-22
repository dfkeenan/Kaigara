namespace Kaigara.Menus;

[AttributeUsage(AttributeTargets.Class)]
public class MenuItemDefinitionAttribute : Attribute
{
    public MenuItemDefinitionAttribute(string name, string locationPath)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (string.IsNullOrEmpty(locationPath))
        {
            throw new ArgumentException($"'{nameof(locationPath)}' cannot be null or empty.", nameof(locationPath));
        }

        Name = name;

        Location = new MenuItemLocation(locationPath);
    }

    public string Name { get; }
    public string? IconName { get; set; }
    public string? Label { get; set; }
    public int DisplayOrder { get; set; }

    public MenuItemLocation Location { get; }
}
