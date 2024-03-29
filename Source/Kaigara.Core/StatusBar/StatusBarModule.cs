using Autofac;
using Kaigara.StatusBar.ViewModels;

namespace Kaigara.StatusBar;
public class StatusBarModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.RegisterType<StatusBarViewModel>()
            .As<IStatusBar>()
            .SingleInstance();
    }
}
