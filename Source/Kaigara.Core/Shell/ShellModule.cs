using System.Reactive.Disposables;
using Autofac;
using Avalonia.Controls;
using Avalonia.Data;
using Dock.Model.Core;
using Kaigara.Commands;
using Kaigara.Shell.Controls;
using Kaigara.Shell.ViewModels;
using ReactiveUI;

namespace Kaigara.Shell;

public sealed class ShellModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        builder.RegisterType<ShellViewModel>()
               .As<IShell>()
               .SingleInstance();

        builder.RegisterType<ShellDockFactory>()
               .As<IFactory>()
               .SingleInstance();

        builder.RegisterCommands<ShellModule>();

        builder.Register(c =>
        {
            ICommandManager commandManager = c.Resolve<ICommandManager>();

            ReactiveHostWindow window = new ReactiveHostWindow()
            {
                [!Window.TitleProperty] = new Binding("ActiveDockable.ActiveDockable.Title")
            };

            window.WhenActivated(d =>
            {
                commandManager
                .SyncKeyBindings(window.KeyBindings)
                .DisposeWith(d);
            });

            return window;

        })
        .As<IHostWindow>()
        .InstancePerDependency();
    }
}
