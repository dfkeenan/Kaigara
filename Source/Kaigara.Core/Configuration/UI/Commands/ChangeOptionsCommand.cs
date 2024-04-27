using Kaigara.Commands;
using Kaigara.Configuration.UI.ViewModels;
using Kaigara.Dialogs;

namespace Kaigara.Configuration.UI.Commands;

[CommandDefinition("Options", IconName = "Settings")]
public class ChangeOptionsCommand(IDialogService dialogService,
                                  ConfigurationManager configuration,
                                  Lazy<OptionsDialogViewModel> optionsDialogViewModel) : ReactiveRegisteredAsyncCommand
{
    private readonly IDialogService dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
    private readonly ConfigurationManager configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    private readonly Lazy<OptionsDialogViewModel> optionsDialogViewModel = optionsDialogViewModel ?? throw new ArgumentNullException(nameof(optionsDialogViewModel));

    protected override async Task OnExecuteAsync(CancellationToken cancellationToken)
    {
        var updates = await dialogService.ShowModal(optionsDialogViewModel.Value);

        if (!updates.Any()) return;

        await configuration.UpdateAsync(updates.Select(u => (u.Metadata.ModelType!, u.ViewModel)));
    }
}
