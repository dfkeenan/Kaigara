using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Kaigara.Commands
{
    public sealed class CommandModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<CommandManager>()
               .As<ICommandManager>()
               .SingleInstance();

        }
    }
}
