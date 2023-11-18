using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Kaigara.Dialogs.ViewModels;
using Kaigara.Dialogs.Views;

namespace Kaigara.Dialogs;
public class DialogService : IDialogService
{
    public Task ShowDialogFor<TDialogViewModel>(TDialogViewModel viewModel, Window owner)
        where TDialogViewModel : class, IDialogViewModel
    {
        var dialog = new DialogView()
        {
            DataContext = viewModel,
        };

        return dialog.ShowDialog(owner);
    }

    public Task<TResult> ShowDialogFor<TResult>(IDialogViewModel<TResult> viewModel, Window owner)
    {
        var dialog = new DialogView()
        {
            DataContext = viewModel,
        };

        return dialog.ShowDialog<TResult>(owner);
    }
}