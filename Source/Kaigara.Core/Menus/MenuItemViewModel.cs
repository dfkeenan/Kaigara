using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Input;
using ReactiveUI;
using Kaigara.ViewModels;

namespace Kaigara.Menus
{
    public class MenuItemViewModel : ReactiveObject, IDisposable
    {
        private ObservableCollection<MenuItemViewModel> items;
        private IDisposable changeSubscription;
        private MenuItemDefinition definition;

        public MenuItemViewModel(MenuItemDefinition definition)
        {
            this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
            items = definition.Items.ToObservableViewModelCollection(d => d.Build());

            changeSubscription = definition.Changed.Subscribe(n => 
            {
                this.RaisePropertyChanged(n.PropertyName);
            });
        }

        public MenuItemDefinition Definition => definition;

        public string Name => definition.Name;
        public string? Label => definition.Label;

        public virtual bool IsVisible => definition.IsVisible;

        public virtual ICommand? Command => definition.Command;
        public virtual KeyGesture? InputGesture => definition.InputGesture;
        public virtual object? CommandParameter => null;
        public IEnumerable<MenuItemViewModel> Items => items;

        public void Dispose()
        {
            changeSubscription.Dispose();

            foreach (var item in items)
            {
                item.Dispose();
            }
        }
    }

    public class MenuItemGroupViewModel : MenuItemViewModel
    {
        public MenuItemGroupViewModel(MenuItemGroupDefinition definition) 
            : base(definition)
        {
        }
    }
}
