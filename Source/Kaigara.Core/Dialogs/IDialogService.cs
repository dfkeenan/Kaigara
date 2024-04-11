using Kaigara.Dialogs.ViewModels;

namespace Kaigara.Dialogs;
public interface IDialogService
{
    Task ShowModal<TDialogViewModel>() where TDialogViewModel : IDialogViewModel;
    Task<TResult> ShowModal<TDialogViewModel, TResult>() where TDialogViewModel : IDialogViewModel<TResult>;
    Task ShowModal(IDialogViewModel viewModel);
    Task<TResult> ShowModal<TResult>(IDialogViewModel<TResult> viewModel);
}