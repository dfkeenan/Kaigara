using Dock.Model.ReactiveUI.Controls;

namespace ExampleApplication.Documents.ViewModels;

public class OtherDocumentViewModel : Document
{
    public OtherDocumentViewModel()
    {
        Id = Guid.NewGuid().ToString();
        Title = "Other Document";
    }
}
