namespace Kaigara.ToolBars
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ToolBarItemDefinitionAttribute : Attribute
    {
        public ToolBarItemDefinitionAttribute(string name, string trayName, string toolBarName)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(trayName))
            {
                throw new ArgumentException($"'{nameof(trayName)}' cannot be null or empty.", nameof(trayName));
            }

            if (string.IsNullOrEmpty(toolBarName))
            {
                throw new ArgumentException($"'{nameof(toolBarName)}' cannot be null or empty.", nameof(toolBarName));
            }

            Name = name;

            Location = new ToolBarItemLocation(trayName, toolBarName);
        }

        public string Name { get; }
        public string? IconName { get; set; }
        public string? Label { get; set; }
        public ToolBarItemLocation Location { get; }
    }
}
