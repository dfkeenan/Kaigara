namespace Kaigara.ToolBars;

public interface IToolBarManager
{
    IDisposable Register(ToolBarTrayDefinition definition);

    IDisposable ConfigureDefinition(ToolBarLocation location, Action<ToolBarDefinition> options);
    IDisposable Register(ToolBarLocation location, ToolBarDefinition definition);

    IDisposable ConfigureDefinition(ToolBarItemLocation location, Action<ToolBarItemDefinition> options);
    IDisposable Register(ToolBarItemLocation location, ToolBarItemDefinition definition);
}
