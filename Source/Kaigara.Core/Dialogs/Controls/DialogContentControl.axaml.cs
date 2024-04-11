using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Kaigara.Dialogs.Controls;
public class DialogContentControl : ContentControl
{
    private readonly ObservableCollection<Button> buttons = [];

    public static readonly DirectProperty<DialogContentControl,IEnumerable<Button>> ButtonsProperty =
        AvaloniaProperty.RegisterDirect<DialogContentControl, IEnumerable<Button>>(nameof(Buttons), c => c.buttons);

    public IEnumerable<Button> Buttons
    {
        get => buttons;
    }
}
