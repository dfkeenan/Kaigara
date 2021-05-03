using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.Core;

namespace Kaigara.Shell
{
    public interface IShell
    {
        IFactory Factory { get; set; }
        IDock Layout { get; set; }
    }
}
