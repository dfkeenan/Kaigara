using Autofac;
using Kaigara.MainWindow;

namespace Kaigara.Hosting
{
    public sealed class DefaultsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder
                .DependsOnModule<MainWindowModule>();
        }
    }
}
