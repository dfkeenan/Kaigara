using Autofac;

namespace Kaigara.Toolbars;

public sealed class ToolbarModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterType<ToolbarManager>()
            .As<IToolbarManager>()
            .SingleInstance();

    }
}
