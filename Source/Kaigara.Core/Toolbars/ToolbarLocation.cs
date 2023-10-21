namespace Kaigara.Toolbars;

public class ToolbarLocation : UIComponentLocation
{
    public ToolbarLocation(string trayName)
        : base(trayName)
    {
        if(IsRelative)
            throw new ArgumentException("Must not be relative path",nameof(trayName));
    }
}
