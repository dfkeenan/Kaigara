using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Kaigara.DependencyInjection;
using Kaigara.Menus;

namespace Kaigara
{
    public class MainWindowModule : ShellAppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            DependsOnModule<MenuModule>(builder);
            RegisterViewModels(builder);
        }

        protected override void OnBuild(ILifetimeScope scope)
        {
            if(scope.TryResolve<IMenuManager>(out var menuManager))
            {

            }
        }
    }
}
