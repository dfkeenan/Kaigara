using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Commands;
using Kaigara.Dialogs.Commands;
using Kaigara.Dialogs.ViewModels;
using Kaigara.Menus;
using Kaigara.Toolbars;

namespace ExampleApplication.Dialogs;

[MenuItemDefinition("ExampleDialog", "MainMenu/File/Example")]
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
