using Kaigara.Menus;

namespace Kaigara.ToolBars;

[AttributeUsage(AttributeTargets.Class)]
public class ToolBarItemDefinitionAttribute : Attribute
{
    public ToolBarItemDefinitionAttribute(string name, string locationPath)
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

        Location = new ToolBarItemLocation(locationPath);
    }

    public string Name { get; }
    public string? IconName { get; set; }
    public string? Label { get; set; }
    public int DisplayOrder { get; set; }
    public ToolBarItemLocation Location { get; }
}
