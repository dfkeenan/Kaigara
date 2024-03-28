using Autofac;
using Autofac.Features.Metadata;
using Kaigara.Commands;
using Kaigara.Dialogs.Commands;
using Kaigara.Dialogs.ViewModels;

namespace Kaigara.Dialogs;
public class DialogsModule : Module
{
    internal const string MetadataName = "DialogAttributes";

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DialogService>()
            .SingleInstance()
            .As<IDialogService>();

        builder.RegisterAdapter<Meta<Func<IDialogViewModel>>, RegisteredCommandBase>((c, f)
            =>
            {
                var attributes
                    = f.Metadata.TryGetValue(MetadataName, out var a)
                        ? a as IEnumerable<Attribute>
                        : Enumerable.Empty<Attribute>();
                return new ShowDialogCommand(c.Resolve<IDialogService>(), f.Value, attributes);
            });
    }
}
