using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Styling;

namespace Kaigara.Avalonia.Controls;
public class MathUpDown : NumericUpDown, IStyleable
{
    Type IStyleable.StyleKey => typeof(MathUpDown);
}
