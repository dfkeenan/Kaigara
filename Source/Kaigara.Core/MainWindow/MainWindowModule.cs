using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls;
using Avalonia.Data;
using Dock.Avalonia.Controls;
using Dock.Model.Core;
using Kaigara.MainWindow.ViewModels;
using Kaigara.Menus;
using Kaigara.Shell;

namespace Kaigara.MainWindow
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

           
            builder.Register(c=> 
            {
                var menu = new MainMenuViewModel();
                c.Resolve<IMenuManager>().Register(menu.Definition); 
                return menu; 
            }).AsSelf().SingleInstance();
        }
            
    }
}
