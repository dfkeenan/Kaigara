using Kaigara.Commands;
using Kaigara.Dialogs.ViewModels;
using Kaigara.Menus;
using Kaigara.Toolbars;

namespace Kaigara.Dialogs.Commands;
public class ShowDialogCommand : RegisteredAsyncCommand, IToolbarItemDefinitionSource, IMenuItemDefinitionSource
{
    private readonly IDialogService dialogService;
    private readonly Func<IDialogViewModel> dialogFactory;
    private readonly IToolbarItemDefinitionSource? toolbarItemDefinitionSource;
    private readonly IMenuItemDefinitionSource? menuItemDefinitionSource;

    public ShowDialogCommand(IDialogService dialogService, Func<IDialogViewModel> dialogFactory, IEnumerable<Attribute>? attributes)
        : base(attributes?.OfType<CommandDefinitionAttribute>()?.FirstOrDefault())
    {
        this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        this.dialogFactory = dialogFactory ?? throw new ArgumentNullException(nameof(dialogFactory));

        if (attributes is not null)
        {
            foreach (var attribute in attributes)
            {
                switch (attribute)
                {
                    case IToolbarItemDefinitionSource toolbarItemDefinitionAttribute:
                        toolbarItemDefinitionSource = toolbarItemDefinitionAttribute;
                        break;
                    case IMenuItemDefinitionSource menuItemDefinitionAttribute:
                        menuItemDefinitionSource = menuItemDefinitionAttribute;
                        break;
                }
            }
        }
    }

    bool IToolbarItemDefinitionSource.IsDefined => toolbarItemDefinitionSource?.IsDefined ?? false;

    ToolbarItemLocation? IToolbarItemDefinitionSource.Location => toolbarItemDefinitionSource?.Location;

    ToolbarItemDefinition? IToolbarItemDefinitionSource.GetDefinition(RegisteredCommandBase? command)
        => toolbarItemDefinitionSource?.GetDefinition(command);

    bool IMenuItemDefinitionSource.IsDefined => menuItemDefinitionSource?.IsDefined ?? false;

    MenuItemLocation? IMenuItemDefinitionSource.Location => menuItemDefinitionSource?.Location;

    MenuItemDefinition? IMenuItemDefinitionSource.GetDefinition(RegisteredCommandBase? command)
        => menuItemDefinitionSource?.GetDefinition(command);

    protected override async Task OnExecuteAsync(CancellationToken cancellationToken)
    {
        var dialog = dialogFactory();
        await dialogService.ShowModal(dialog);
    }
}
