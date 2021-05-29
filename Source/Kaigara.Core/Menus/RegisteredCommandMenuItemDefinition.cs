using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Avalonia.Input;
using Kaigara.Commands;
using ReactiveUI;

namespace Kaigara.Menus
{
    public interface IRegisteredCommandMenuItemDefinition
    {
        void BindCommand(IComponentContext context);
    }

    public class MenuItemDefinition<TCommand> : MenuItemDefinition, IRegisteredCommandMenuItemDefinition 
        where TCommand : RegisteredCommand
    {
        public MenuItemDefinition(string name, string? label = null)
            : base(name, label)
        {
        }

        private TCommand? registeredCommand;

        public override string? Label => base.Label ?? registeredCommand?.Label;

        public override ICommand? Command => registeredCommand?.Command;
        public override KeyGesture? InputGesture => registeredCommand?.InputGesture;

        void IRegisteredCommandMenuItemDefinition.BindCommand(IComponentContext context)
        {
            registeredCommand ??= context.Resolve<TCommand>();

            this.RaisePropertyChanged(nameof(Label));
            this.RaisePropertyChanged(nameof(Command));
            this.RaisePropertyChanged(nameof(InputGesture));
        }
    }
}
