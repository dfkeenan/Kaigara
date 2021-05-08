using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Data;
using Dock.Avalonia.Controls;
using Dock.Model.Core;
using Kaigara.Menus;
using Kaigara.Shell;

namespace Kaigara
{
    public class MainWindowModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder
                .DependsOnModule<MenuModule>()
                .DependsOnModule<ShellModule>()
                .RegisterViewModels<MainWindowModule>();


            builder.Register<IHostWindow>(c =>
            {
                var hostWindow = new HostWindow()
                {
                    [!HostWindow.TitleProperty] = new Binding("ActiveDockable.Title")
                };
                return hostWindow;
            });
        }
    }
}
