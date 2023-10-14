using Avalonia.Controls;
using Avalonia.Styling;

namespace Kaigara.Avalonia.Controls;
public class MathUpDown : NumericUpDown
{
    protected override Type StyleKeyOverride => typeof(MathUpDown);
}
