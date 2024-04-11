using Kaigara.About.ViewModels;
using Kaigara.Commands;
using Kaigara.Dialogs;

namespace Kaigara.About.Commands;


public class ShowAboutDialogCommand : RegisteredAsyncCommand
{
    private readonly ApplicationInfo applicationInfo;
    private readonly IDialogService dialogService;

    public ShowAboutDialogCommand(ApplicationInfo applicationInfo, IDialogService dialogService)
    {
        this.applicationInfo = applicationInfo;
        this.dialogService = dialogService;

        Label = $"_About {applicationInfo.ProductName}";
    }

    protected override Task OnExecuteAsync()
    {
        return dialogService.ShowModal<IAboutDialogViewModel>();
    }
}
