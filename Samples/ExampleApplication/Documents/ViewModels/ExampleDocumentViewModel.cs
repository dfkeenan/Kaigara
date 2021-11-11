using Dock.Model.ReactiveUI.Controls;
using Kaigara.Menus;
using ReactiveUI;

namespace ExampleApplication.Documents.ViewModels;

public class ExampleDocumentViewModel : Document
{
    public ExampleDocumentViewModel(IMenuManager menuManager)
    {
        Id = Guid.NewGuid().ToString();
        Title = "Example Document";
    }

    private bool ischecked;

    public bool IsChecked
    {
        get { return ischecked; }
        set { this.RaiseAndSetIfChanged(ref ischecked, value); }
    }

}
