using Autofac;

namespace Kaigara.About;
public class AboutModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterCommands<AboutModule>();
    }
}
