using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Input;
using ExampleApplication.Tools.ViewModels;
using Kaigara.Commands;
using Kaigara.Shell;

namespace ExampleApplication.Commands
{
    //[CommandDefinition("Example Command",  IconName = "ExampleIcon")]
    [CommandDefinition("Example Command", DefaultInputGesture = "Ctrl+E", IconName = "ExampleIcon")]
    public class ExampleCommand : RegisteredCommand
    {
        private readonly IShell shell;

        public ExampleCommand(IShell shell)
        {
            this.shell = shell;
        }
        protected override void OnExecute()
        {
            Debug.WriteLine($"Hello {Label} - {Name}, {InputGesture}");
            shell.Tools.Open<IRightToolViewModel>();
            shell.Tools.Open<LeftToolViewModel>();
            shell.Tools.Open(new BottomToolViewModel());
        }
    }
}
