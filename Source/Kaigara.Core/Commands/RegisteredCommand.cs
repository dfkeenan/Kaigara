using System.Windows.Input;
using Avalonia.Input;
using Avalonia.Threading;
using ReactiveUI;

namespace Kaigara.Commands
{
    public abstract class RegisteredCommand : RegisteredCommandBase
    {
        public RegisteredCommand()
            :base()
        {

        }

        protected RegisteredCommand(string name, string label, KeyGesture? keyGesture)
            : base(name, label, keyGesture)
        {

        }

        protected override ICommand CreateCommand()
        {
            return ReactiveCommand.Create(OnExecute,CanExecute, AvaloniaScheduler.Instance);
        }

        protected abstract void OnExecute();
    }
}
