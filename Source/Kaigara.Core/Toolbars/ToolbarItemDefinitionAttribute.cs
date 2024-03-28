using Kaigara.Commands;

namespace Kaigara.Toolbars;

[AttributeUsage(AttributeTargets.Class)]
public class ToolbarItemDefinitionAttribute : Attribute, IToolbarItemDefinitionSource
{
    public ToolbarItemDefinitionAttribute(string name, string locationPath)
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

        Location = new ToolbarItemLocation(locationPath);
    }

    public string Name { get; }
    public string? IconName { get; set; }
    public string? Label { get; set; }
    public int DisplayOrder { get; set; }
    public ToolbarItemLocation Location { get; }

    bool IToolbarItemDefinitionSource.IsDefined => true;

    public ToolbarItemDefinition? GetDefinition(RegisteredCommandBase? command)
    {
        var definition = new ToolbarItemDefinition(Name, Label, IconName, DisplayOrder)
        {
            RegisteredCommand = command
        };

        return definition;
    }
}
