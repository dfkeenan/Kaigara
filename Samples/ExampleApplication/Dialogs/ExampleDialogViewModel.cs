using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Dialogs.ViewModels;

namespace ExampleApplication.Dialogs;
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
