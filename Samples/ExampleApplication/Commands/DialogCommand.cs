using System.Diagnostics;
using ExampleApplication.Dialogs;
using Kaigara.Commands;
using Kaigara.Dialogs;
using Kaigara.Toolbars;

namespace ExampleApplication.Commands;

[ToolbarItemDefinition("Dialog", "MainToolbarTray/Example")]
[CommandDefinition("Show Dialog Command", IconName = "Dialog")]
public class DialogCommand : RegisteredAsyncCommand
{
    public required IDialogService DialogService { get; init; }

    protected override async Task OnExecuteAsync()
    {
        var result = await DialogService.ShowModal<ExampleDialogViewModel, bool>();
        Debug.WriteLine(result);
    }
}
