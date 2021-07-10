﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Avalonia.Input;
using Kaigara.Collections.ObjectModel;
using Kaigara.Commands;
using ReactiveUI;

namespace Kaigara.Menus
{
    public class MenuItemDefinition : ReactiveObject, IUIComponentDefinition<MenuItemDefinition>, IEnumerable<MenuItemDefinition>, IDisposable
    {
        private string? label;
        private string? iconName;
        private bool isVisible;
        private readonly ObservableCollection<MenuItemDefinition> items;
        private RegisteredCommandBase? registeredCommand;
        private List<Action<IComponentContext>>? bindings;
        private CompositeDisposable disposables;

        public MenuItemDefinition(string name, string? label = null, string? iconName = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            this.label = label;
            this.iconName = iconName;
            items = new ObservableCollection<MenuItemDefinition>();
            Items = items.AsReadOnlyObservableCollection();
            disposables = new CompositeDisposable();
            isVisible = true;
        }

        public string Name { get; }

        public virtual string? Label => label ?? registeredCommand?.Label;

        public virtual string? IconName => iconName ?? registeredCommand?.IconName;

        public virtual bool IsVisible 
        { 
            get => isVisible;
            set => this.RaiseAndSetIfChanged(ref isVisible, value); 
        }

        public virtual ICommand? Command => registeredCommand?.Command;
        public virtual KeyGesture? InputGesture => registeredCommand?.InputGesture;

        public ReadOnlyObservableCollection<MenuItemDefinition> Items { get; }

        protected ICollection<Action<IComponentContext>> Bindings
        {
            get
            {
                bindings ??= new List<Action<IComponentContext>>();
                return bindings;
            }
        }

        public void Add(MenuItemDefinition menuItemDefinition)
        {
            items.Add(menuItemDefinition);
        }

        public bool Remove(MenuItemDefinition menuItemDefinition)
        {
            return items.Remove(menuItemDefinition);
        }

        IEnumerator<MenuItemDefinition> IEnumerable<MenuItemDefinition>.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public MenuItemDefinition BindCommand<TCommand>()
            where TCommand : RegisteredCommandBase
        {
            return BindCommand(typeof(TCommand));
        }

        public MenuItemDefinition BindCommand(Type type)
        {
            if (!typeof(RegisteredCommandBase).IsAssignableFrom(type))
            {
                throw new ArgumentOutOfRangeException(nameof(type), $"The Type must derive from {nameof(RegisteredCommandBase)}");
            }


            Bindings.Add(context =>
            {
                registeredCommand ??= (RegisteredCommandBase)context.Resolve(type);
                this.RaisePropertyChanged(nameof(Label));
                this.RaisePropertyChanged(nameof(Command));
                this.RaisePropertyChanged(nameof(InputGesture));
            });

            return this;
        }

        public MenuItemDefinition VisibleWhen<TSource>(Func<TSource, IObservable<bool>> selector)
             where TSource : notnull
        {
            Bindings.Add(context =>
            {
                var source = context.Resolve<TSource>();
                selector(source).BindTo(this, o => o.IsVisible).DisposeWith(disposables);
                this.RaisePropertyChanged(nameof(isVisible));
            });

            return this;
        }

        public void UpdateBindings(IComponentContext context)
        {
            bindings?.ForEach(a => a(context));
        }

        internal virtual IMenuItemViewModel Build()
            => new DefinedMenuItemViewModel(this);

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
