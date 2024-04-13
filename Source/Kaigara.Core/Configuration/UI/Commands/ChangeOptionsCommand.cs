using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Commands;
using Kaigara.Configuration.UI.ViewModels;
using Kaigara.Dialogs;
using Microsoft.Extensions.Configuration;

namespace Kaigara.Configuration.UI.Commands;

[CommandDefinition("Options", IconName = "Settings")]
public class ChangeOptionsCommand(IDialogService dialogService,
                                  IConfiguration configuration,
                                  Lazy<OptionsDialogViewModel> optionsDialogViewModel) : RegisteredAsyncCommand
{
    private readonly IDialogService dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
    private readonly IConfiguration configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private readonly Lazy<OptionsDialogViewModel> optionsDialogViewModel = optionsDialogViewModel ?? throw new ArgumentNullException(nameof(optionsDialogViewModel));

    protected override async Task OnExecuteAsync()
    {
        var update = await dialogService.ShowModal(optionsDialogViewModel.Value);

        if (!update) return;


    }
}
