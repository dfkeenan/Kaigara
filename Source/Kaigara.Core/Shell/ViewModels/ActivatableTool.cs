using System.Reactive.Disposables;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;

namespace Kaigara.Shell.ViewModels;
public class ActivatableTool : Tool, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    public ActivatableTool()
    {
        this.WhenActivated(OnActivated);
    }

    protected virtual void OnActivated(CompositeDisposable disposables)
    {
        Disposable.Create(OnDispose).DisposeWith(disposables);
    }

    protected virtual void OnDispose()
    {

    }
}
