using System.Windows.Input;
using Dock.Model.ReactiveUI.Controls;
using ExampleApplication.Documents.ViewModels;
using Kaigara.Commands;
using Kaigara.Menus;

namespace ExampleApplication.Commands;

[MenuItemDefinition("ExampleDocumentContextMenu", "DocumentContextMenu", CanExecuteBehavior = Kaigara.CanExecuteBehavior.Visible)]
[CommandDefinition("Example Document Command", IconName = "Run")]
public class ExampleDocumentContextCommand : RegisteredCommand<Document>
{
    protected override void OnExecute(Document param)
    {
        
    }

    //private bool IsDocumetnOK(Document param)
    //{
    //    return param is ExampleDocumentViewModel;
    //}

    //protected override ICommand CreateCommand()
    //{
    //    return base.CreateCommand().OverrideCanExecute<Document>(IsDocumetnOK);
    //}
}