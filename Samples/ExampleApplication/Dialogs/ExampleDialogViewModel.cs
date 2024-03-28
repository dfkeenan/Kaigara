using Kaigara.Dialogs.Commands;
using Kaigara.Dialogs.ViewModels;
using Kaigara.Menus;
using Kaigara.Toolbars;

namespace ExampleApplication.Dialogs;

[MenuItemDefinition("ExampleDialog", "/File/Example")]
[ToolbarItemDefinition("Dialog", "/Example")]
[ShowDialogCommandDefinition("Example Dialog", IconName = "Dialog")]
public class ExampleDialogViewModel : DialogViewModel<bool>
{
    public ExampleDialogViewModel()
    {
        Title = "ExampleDialog";
        Width = 200;
        Height = 200;
    }

    public Task Ok() => Close(true);

    public Task Cancel() => Close(false);

}
