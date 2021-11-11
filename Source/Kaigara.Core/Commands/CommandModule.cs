using Autofac;

namespace Kaigara.Commands;

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
