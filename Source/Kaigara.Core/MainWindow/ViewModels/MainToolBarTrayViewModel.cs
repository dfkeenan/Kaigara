using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.ToolBars;

namespace Kaigara.MainWindow.ViewModels
{
    public class MainToolBarTrayViewModel : ToolBarTrayViewModel
    {
        protected override ToolBarTrayDefinition CreateDefinition()
            => new ToolBarTrayDefinition("MainToolBarTray")
            {

            };
    }
}
