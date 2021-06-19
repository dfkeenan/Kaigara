using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Avalonia.Input;
using ReactiveUI;

namespace Kaigara.Menus
{
    internal class DefinedMenuItemViewModelBase: ReactiveObject, IDisposable, IMenuItemViewModel
    {
        private MenuItemDefinition definition;

        public DefinedMenuItemViewModelBase(MenuItemDefinition definition)
        {
            this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        public MenuItemDefinition Definition => definition;

        public string Name => definition.Name;
        public string? Label => definition.Label;

        public virtual bool IsVisible => definition.IsVisible;

        public virtual ICommand? Command => definition.Command;
        public virtual KeyGesture? InputGesture => definition.InputGesture;
        public virtual object? CommandParameter => null;
        public virtual IEnumerable<IMenuItemViewModel> Items => Enumerable.Empty<IMenuItemViewModel>();

        public virtual void Dispose()
        {
            
        }
    }
}
