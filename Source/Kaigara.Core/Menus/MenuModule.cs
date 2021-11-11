using Autofac;

namespace Kaigara.Menus;

public sealed class MenuModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterType<MenuManager>()
            .As<IMenuManager>()
            .SingleInstance();

    }
}
