using Autofac;

namespace Kaigara.ToolBars
{
    public sealed class ToolBarModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ToolBarManager>()
                .As<IToolBarManager>()
                .SingleInstance();

        }
    }
}
