using System.Reactive.Linq;
using Dock.Model.ReactiveUI.Controls;
using ExampleApplication.Documents.ViewModels;
using Kaigara.Commands;
using Kaigara.Menus;
using Kaigara.Shell;

namespace ExampleApplication.Commands;

[MenuItemDefinition("ExampleDocumentContextMenu", "DocumentContextMenu", CanExecuteBehavior = CanExecuteBehavior.Visible)]
[CommandDefinition("Example Document Command", IconName = "Run")]
public class ExampleDocumentContextCommand : ReactiveRegisteredCommand<Document>
{
    public required IShell Shell { get; init; }

    protected override void OnRegistered(ICommandManager commandManager)
    {
        commandManager.RequerySuggested
            .Subscribe(e => NotifyCanExecuteChanged());
    }

    protected override bool CanExecute(Document? parameter)
    {
        return parameter is ExampleDocumentViewModel;
    }

    protected override void OnExecute(Document? param)
    {

    }
}