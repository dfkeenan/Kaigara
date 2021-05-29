using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Input;
using Kaigara.Commands;

namespace ExampleApplication.Commands
{
    [CommandDefinition("Example Command", DefaultInputGesture = "Ctrl+E")]
    public class ExampleCommand : RegisteredCommand
    {
        protected override void OnExecute()
        {
            Debug.WriteLine($"Hello {Label} - {Name}, {InputGesture}");
        }
    }
}
