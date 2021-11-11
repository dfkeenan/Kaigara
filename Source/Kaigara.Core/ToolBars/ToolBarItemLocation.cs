namespace Kaigara.ToolBars
{
    public class ToolBarItemLocation : UIComponentLocation
    {
        public ToolBarItemLocation(string trayName, string toolBarName)
            : base(BuildPath(trayName, toolBarName))
        {
            
        }

        private static string[] BuildPath(string trayName, string toolBarName)
        {
            if (trayName is null)
            {
                throw new ArgumentNullException(nameof(trayName));
            }

            if (toolBarName is null)
            {
                throw new ArgumentNullException(nameof(toolBarName));
            }

            return new string[] { trayName, toolBarName };
        }
    }
}