using System.Reactive.Disposables;
using System.Windows.Input;
using Autofac;
using Avalonia.Input;
using Kaigara.Commands;
using ReactiveUI;

namespace Kaigara
{
    public abstract class UIComponentItemDefinition<T> : ReactiveObject, IDisposable
        where T : UIComponentItemDefinition<T>
    {
        private string? label;
        private string? iconName;
        private bool isVisible;
        private RegisteredCommandBase? registeredCommand;
        private List<Action<IComponentContext>>? bindings;
        private CompositeDisposable disposables;

        public UIComponentItemDefinition(string name, string? label = null, string? iconName = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.label = label;
            this.iconName = iconName;
            disposables = new CompositeDisposable();
            isVisible = true;
        }

        public string Name { get; }

        public string? Label => label ?? registeredCommand?.Label;

        public string? IconName => iconName ?? registeredCommand?.IconName;

        public bool IsVisible
        {
            get => isVisible;
            set => this.RaiseAndSetIfChanged(ref isVisible, value);
        }

        public ICommand? Command => registeredCommand?.Command;
        public KeyGesture? InputGesture => registeredCommand?.InputGesture;

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
            }
        }

        public virtual T BindCommand<TCommand>()
           where TCommand : RegisteredCommandBase
        {
            return BindCommand(typeof(TCommand));
        }

        public virtual T BindCommand(Type type)
        {
            if (!typeof(RegisteredCommandBase).IsAssignableFrom(type))
            {
                throw new ArgumentOutOfRangeException(nameof(type), $"The Type must derive from {nameof(RegisteredCommandBase)}");
            }


            Bindings.Add(context =>
            {
                RegisteredCommand ??= (RegisteredCommandBase)context.Resolve(type);
            });

            return (T)this;
        }

        public virtual T VisibleWhen<TSource>(Func<TSource, IObservable<bool>> selector)
             where TSource : notnull
        {
            Bindings.Add(context =>
            {
                var source = context.Resolve<TSource>();
                selector(source).BindTo(this, o => o.IsVisible).DisposeWith(disposables);
                this.RaisePropertyChanged(nameof(IsVisible));
            });

            return (T)this;
        }

        public void UpdateBindings(IComponentContext context)
        {
            bindings?.ForEach(a => a(context));
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            disposables.Dispose();
        }
    }
}
