using Avalonia.Controls;
using Avalonia.Styling;

namespace Kaigara.Avalonia.Controls;
public class MathUpDown : NumericUpDown, IStyleable
{
    Type IStyleable.StyleKey => typeof(MathUpDown);
}
