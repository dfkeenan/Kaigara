using System.Reactive.Disposables;
using ReactiveUI;

namespace Kaigara.ViewModels;

public abstract class ActivatableViewModel : ViewModel, IActivatableViewModel
{
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    public ActivatableViewModel()
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
