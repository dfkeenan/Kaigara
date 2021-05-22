using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaigara.Menus
{
    public interface ILabeledMenuItem
    {
        string Label { get; }
        string IconSource { get; }
    }
}
