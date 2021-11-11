namespace Kaigara.ToolBars
{
    public class ToolBarLocation : UIComponentLocation
    {
        public ToolBarLocation(string trayName)
            : base(BuildPath(trayName))
        {

        }

        private static string[] BuildPath(string trayName)
        {
            if (trayName is null)
            {
                throw new ArgumentNullException(nameof(trayName));
            }

            return new string[] { trayName };
        }
    }
}