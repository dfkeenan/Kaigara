using Avalonia.ReactiveUI;
using ExampleApplication.Documents.ViewModels;

namespace ExampleApplication.Documents.Views;

public partial class ExampleDocumentView : ReactiveUserControl<ExampleDocumentViewModel>
{
    public ExampleDocumentView()
    {
        InitializeComponent();
    }
}
