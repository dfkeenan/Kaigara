using Autofac;
using Kaigara.MainWindow;

namespace Kaigara;

public sealed class CoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder
            .DependsOnModule<MainWindowModule>();
    }
}
