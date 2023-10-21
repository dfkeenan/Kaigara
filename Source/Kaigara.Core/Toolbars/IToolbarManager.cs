namespace Kaigara.Toolbars;

public interface IToolbarManager
{
    IDisposable Register(ToolbarTrayDefinition definition);

    IDisposable ConfigureDefinition(ToolbarLocation location, Action<ToolbarDefinition> options);
    IDisposable Register(ToolbarLocation location, ToolbarDefinition definition);

    IDisposable ConfigureDefinition(ToolbarItemLocation location, Action<ToolbarItemDefinition> options);
    IDisposable Register(ToolbarItemLocation location, ToolbarItemDefinition definition);
}
