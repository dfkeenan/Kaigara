namespace Kaigara.ToolBars;

public class ToolBarLocation : UIComponentLocation
{
    public ToolBarLocation(string trayName)
        : base(trayName)
    {
        if(IsRelative)
            throw new ArgumentException("Must not be relative path",nameof(trayName));
    }
}
