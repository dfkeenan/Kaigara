using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Data;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Shell.Controls;
using Kaigara.Shell.ViewModels;
using ReactiveUI;

namespace Kaigara.Shell
{
    public class ShellDockFactory : Factory
    {
        private readonly Func<IHostWindow> hostWindowFactory;
        private IRootDock? rootDock;
        private IDocumentDock? mainDocumentsDock;

        public ShellDockFactory(Func<IHostWindow> hostWindowFactory)
        {
            if (hostWindowFactory is null)
            {
                throw new ArgumentNullException(nameof(hostWindowFactory));
            }

            this.hostWindowFactory = hostWindowFactory;
        }

        public override IRootDock CreateLayout()
        {
            var mainDocumentsDock = new DocumentDock
            {
                Id = "DocumentsPane",
                Title = "DocumentsPane",
                Proportion = double.NaN,
                IsCollapsable = false,
                VisibleDockables = CreateList<IDockable>
                                    (

                                    )
            };

            var mainLayout = new ProportionalDock
            {
                Id = "MainLayout",
                Title = "MainLayout",
                Proportion = double.NaN,
                Orientation = Orientation.Horizontal,
                ActiveDockable = null,
                VisibleDockables = base.CreateList<IDockable>
                (
                    mainDocumentsDock
                )
            };

            var rootDock = CreateRootDock();

            rootDock.Id = "Root";
            rootDock.Title = "Root";
            rootDock.ActiveDockable = mainLayout;
            rootDock.DefaultDockable = mainLayout;
            rootDock.VisibleDockables = CreateList<IDockable>(mainLayout);

            this.rootDock = rootDock;
            this.mainDocumentsDock = mainDocumentsDock;

            return rootDock;
        }

        public override IRootDock CreateRootDock()
        {
            return new ShellRootDockViewModel();
        }

        public override void InitLayout(IDockable layout)
        {
            this.ContextLocator = new Dictionary<string, Func<object>>
            {
                
            };

            this.HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
            {
                [nameof(IDockWindow)] = hostWindowFactory
            };

            this.DockableLocator = new Dictionary<string, Func<IDockable?>>
            {
                ["Root"] = () => rootDock,
                ["Documents"] = ()=> mainDocumentsDock
            };

            base.InitLayout(layout);
        }
    }
}
