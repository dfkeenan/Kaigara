using System;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Shell;

namespace ExampleApplication.Tools.ViewModels
{
    [Tool(ShellDockIds.LeftToolDock)]
    public class LeftToolViewModel : Tool
    {
        public LeftToolViewModel()
        {
            Id = Guid.NewGuid().ToString();
            Title = "Left Tool";
        }
    }
}
