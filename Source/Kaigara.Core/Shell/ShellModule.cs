using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Dock.Model.Core;
using Kaigara.Commands;
using Kaigara.Shell.ViewModels;

namespace Kaigara.Shell
{
    public sealed class ShellModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.DependsOnModule<CommandModule>();

            builder.RegisterType<ShellViewModel>()
                   .As<IShell>()
                   .SingleInstance();

            builder.RegisterType<ShellDockFactory>()
                   .As<IFactory>()
                   .SingleInstance();
        }
    }
}
