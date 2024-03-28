using Kaigara.ViewModels;

namespace Kaigara.Dialogs.ViewModels;
public interface IDialogViewModel : IWindowViewModel
{
}

public interface IDialogViewModel<TResult> : IDialogViewModel
{
}
