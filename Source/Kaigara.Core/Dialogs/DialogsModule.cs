using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Kaigara.Commands;
using Kaigara.MainWindow.ViewModels;
using Kaigara.MainWindow;
using Kaigara.Menus;
using Kaigara.Shell;
using Kaigara.ToolBars;

namespace Kaigara.Dialogs;
public class DialogsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DialogService>()
            .SingleInstance()
            .As<IDialogService>();
    }
}
