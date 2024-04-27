using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using Autofac;
using Avalonia.Input;
using Kaigara.Commands;
using ReactiveUI;

namespace Kaigara;

public abstract class UIComponentItemDefinition<T> : ReactiveObject, IDisposable
    where T : UIComponentItemDefinition<T>
{
    public static Comparer<T> DisplayOrderComparer { get; }
        = Comparer<T>.Create((x, y) => x.DisplayOrder.CompareTo(y.DisplayOrder));

    private string? label;
    private string? iconName;
    private CanExecuteBehavior canExecuteBehavior;
    private bool isVisible;
    private RegisteredCommandBase? registeredCommand;
    private List<Action<IComponentContext>>? bindings;
    private CompositeDisposable disposables;
    private CompositeDisposable bindingDisposables;
    private IDisposable? canExecuteBinding;

    public UIComponentItemDefinition(string name, string? label = null, string? iconName = null, int displayOrder = 0, CanExecuteBehavior canExecuteBehavior = CanExecuteBehavior.Enabled)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        this.label = label;
        this.iconName = iconName;
        disposables = new CompositeDisposable();
        bindingDisposables = new CompositeDisposable();
        isVisible = true;
        DisplayOrder = displayOrder;
        this.canExecuteBehavior = canExecuteBehavior;
    }

    public string Name { get; }

    public string? Label => label ?? registeredCommand?.Label ?? Name;

    public string? IconName => iconName ?? registeredCommand?.IconName;

    public int DisplayOrder { get; }

    public bool IsVisible
    {
        get => isVisible;
        set => this.RaiseAndSetIfChanged(ref isVisible, value);
    }

    public ICommand? Command => registeredCommand?.Command;
    public KeyGesture? InputGesture => registeredCommand?.InputGesture;


    public CanExecuteBehavior CanExecuteBehavior => canExecuteBehavior;

    protected ICollection<Action<IComponentContext>> Bindings
    {
        get
        {
            bindings ??= new List<Action<IComponentContext>>();
            return bindings;
        }
    }

    public RegisteredCommandBase? RegisteredCommand
    {
        get => registeredCommand;
        set
        {
            registeredCommand = value;
            this.RaisePropertyChanged(nameof(Label));
            this.RaisePropertyChanged(nameof(Command));
            this.RaisePropertyChanged(nameof(InputGesture));

            canExecuteBinding?.Dispose();

            //canExecuteBinding = registeredCommand
            //    ?.CanExecute
            //    ?.Where(c => canExecuteBehavior == CanExecuteBehavior.Visible)
            //    ?.BindTo(this, t => t.IsVisible);
        }
    }

    //public virtual T BindCommand<TCommand>()
    //   where TCommand : RegisteredCommandBase
    //{
    //    return BindCommand(typeof(TCommand));
    //}

    //public virtual T BindCommand(Type type)
    //{
    //    if (!typeof(RegisteredCommandBase).IsAssignableFrom(type))
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(type), $"The Type must derive from {nameof(RegisteredCommandBase)}");
    //    }


    //    Bindings.Add(context =>
    //    {
    //        RegisteredCommand ??= (RegisteredCommandBase)context.Resolve(type);
    //    });

    //    return (T)this;
    //}

    public virtual T VisibleWhen<TSource>(Func<TSource, IObservable<bool>> selector)
         where TSource : notnull
    {
        Bindings.Add(context =>
        {
            var source = context.Resolve<TSource>();
            selector(source)
                .BindTo(this, o => o.IsVisible)
                .DisposeWith(bindingDisposables);
            this.RaisePropertyChanged(nameof(IsVisible));
        });

        return (T)this;
    }

    public void UpdateBindings(IComponentContext context)
    {
        bindingDisposables.Dispose();
        bindingDisposables = new CompositeDisposable();
        bindings?.ForEach(a => a(context));
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        disposables.Dispose();
        bindingDisposables.Dispose();
    }

    
}
