using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Kaigara.DependencyInjection;

namespace Kaigara.Menus
{
    public sealed class MenuModule : ShellAppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<MenuManager>()
                .As<IMenuManager>()
                .SingleInstance();

        }
    }
}
