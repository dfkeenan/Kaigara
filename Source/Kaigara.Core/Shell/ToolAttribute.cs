namespace Kaigara.Shell
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ToolAttribute : Attribute
    {
        public ToolAttribute(string defaultDockId)
        {
            DefaultDockId = defaultDockId ?? throw new ArgumentNullException(nameof(defaultDockId));
        }

        public string DefaultDockId { get; }
    }
}
