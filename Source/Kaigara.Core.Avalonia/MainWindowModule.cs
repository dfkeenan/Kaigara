using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Data;
using Dock.Avalonia.Controls;
using Dock.Model.Core;
using Kaigara.DependencyInjection;
using Kaigara.Menus;
using Kaigara.Shell;

namespace Kaigara
{
    public class MainWindowModule : ShellAppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            DependsOnModule<MenuModule>(builder);
            DependsOnModule<ShellModule>(builder);
            RegisterViewModels(builder);

            builder.Register<IHostWindow>(c =>
            {
                var hostWindow = new HostWindow()
                {
                    [!HostWindow.TitleProperty] = new Binding("ActiveDockable.Title")
                };
                return hostWindow;
            });
        }

        protected override void OnBuild(ILifetimeScope scope)
        {
            if (scope.TryResolve<IMenuManager>(out var menuManager))
            {

            }
        }
    }
}
