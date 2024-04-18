using Kaigara.ViewModels;

namespace Kaigara.Dialogs.ViewModels;
public interface IDialogViewModel : IWindowViewModel
{
    object? DefaultResult { get; }
}

public interface IDialogViewModel<TResult> : IDialogViewModel
{
    
}
