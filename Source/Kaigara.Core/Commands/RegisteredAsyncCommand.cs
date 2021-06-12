using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Input;
using Avalonia.Threading;
using ReactiveUI;

namespace Kaigara.Commands
{
    public abstract class RegisteredAsyncCommand : RegisteredCommandBase
    {
        public RegisteredAsyncCommand()
           : base()
        {

        }

        protected RegisteredAsyncCommand(string name, string label, KeyGesture? keyGesture)
            : base(name, label, keyGesture)
        {
        }

        protected override ICommand CreateCommand()
        {
            return ReactiveCommand.CreateFromTask(OnExecuteAsync, CanExecute, AvaloniaScheduler.Instance);
        }

        protected abstract Task OnExecuteAsync();
    }

    public abstract class RegisteredAsyncCommand<TParam> : RegisteredCommandBase
    {
        public RegisteredAsyncCommand()
           : base()
        {

        }

        protected RegisteredAsyncCommand(string name, string label, KeyGesture? keyGesture)
            : base(name, label, keyGesture)
        {

        }

        protected override ICommand CreateCommand()
        {
            return ReactiveCommand.CreateFromTask<TParam>(OnExecuteAsync, CanExecute, AvaloniaScheduler.Instance);
        }

        protected abstract Task OnExecuteAsync(TParam param);
    }
}
