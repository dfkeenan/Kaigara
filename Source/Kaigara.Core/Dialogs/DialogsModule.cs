using Autofac;

namespace Kaigara.Dialogs;
public class DialogsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DialogService>()
            .SingleInstance()
            .As<IDialogService>();
    }
}
