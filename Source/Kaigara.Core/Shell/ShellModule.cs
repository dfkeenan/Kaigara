using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Dock.Model.Core;
using Kaigara.DependencyInjection;
using Kaigara.Shell.ViewModels;

namespace Kaigara.Shell
{
    public sealed class ShellModule : ShellAppModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ShellViewModel>()
                   .As<IShell>()
                   .SingleInstance();

            builder.RegisterType<ShellDockFactory>()
                   .As<IFactory>()
                   .SingleInstance();
        }
    }
}
