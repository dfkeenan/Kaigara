using System.Reactive.Disposables;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;

namespace Kaigara.Shell.ViewModels;
public class ActivatableDocument : Document, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    public ActivatableDocument()
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
