using Kaigara.Commands;

namespace Kaigara.Menus;

[AttributeUsage(AttributeTargets.Class)]
public class MenuItemDefinitionAttribute : Attribute, IMenuItemDefinitionSource
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

    bool IMenuItemDefinitionSource.IsDefined => true;

    public MenuItemDefinition? GetDefinition(RegisteredCommandBase? command)
    {
        var definition = new MenuItemDefinition(Name, Label, IconName, DisplayOrder)
        {
            RegisteredCommand = command
        };

        return definition;
    }
}
