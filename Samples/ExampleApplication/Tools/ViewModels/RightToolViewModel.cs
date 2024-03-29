﻿using Dock.Model.ReactiveUI.Controls;
using Kaigara.Shell;

namespace ExampleApplication.Tools.ViewModels;

public interface IRightToolViewModel
{
    void DoSomething();
}

[Tool(ShellDockIds.RightToolDock)]
public class RightToolViewModel : Tool, IRightToolViewModel
{
    public RightToolViewModel()
    {
        Id = Guid.NewGuid().ToString();
        Title = "Right Tool";
    }

    public void DoSomething()
    {

    }
}
