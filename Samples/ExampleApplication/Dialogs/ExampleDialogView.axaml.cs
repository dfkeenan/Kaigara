using Avalonia.Controls;
using Avalonia.ReactiveUI;

namespace ExampleApplication.Dialogs;
public partial class ExampleDialogView : ReactiveUserControl<ExampleDialogViewModel>
{
    public ExampleDialogView()
    {
        InitializeComponent();
    }
}
