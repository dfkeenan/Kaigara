using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Kaigara.Dialogs.ViewModels;
using Kaigara.Dialogs.Views;

namespace Kaigara.Dialogs;
public class DialogService(Application application, ILifetimeScope container) : IDialogService
{
    private readonly ILifetimeScope container = container ?? throw new ArgumentNullException(nameof(container));
    private readonly IClassicDesktopStyleApplicationLifetime lifeTime
        = (application.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime) ?? throw new ArgumentException(nameof(application));

    public Task ShowModal<TDialogViewModel>()
        where TDialogViewModel : IDialogViewModel
        => this.ShowModal(container.Resolve<TDialogViewModel>());

    public Task<TResult> ShowModal<TDialogViewModel, TResult>()
        where TDialogViewModel : IDialogViewModel<TResult>
        => this.ShowModal(container.Resolve<TDialogViewModel>());


    public Task ShowModal(IDialogViewModel viewModel)
    {
        if (lifeTime.MainWindow is null) throw new InvalidOperationException("No MainWindow");

        var dialog = new DialogView()
        {
            DataContext = viewModel,
        };


        return dialog.ShowDialog(lifeTime.MainWindow);
    }

    public async Task<TResult> ShowModal<TResult>(IDialogViewModel<TResult> viewModel)
    {
        if (lifeTime.MainWindow is null) throw new InvalidOperationException("No MainWindow");

        var dialog = new DialogView()
        {
            DataContext = viewModel,
        };

        var result = await dialog.ShowDialog<TResult>(lifeTime.MainWindow);
        return result ?? (TResult)viewModel.DefaultResult!;
    }
}