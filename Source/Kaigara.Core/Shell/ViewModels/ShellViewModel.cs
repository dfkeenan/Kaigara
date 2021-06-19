using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Dock.Model.Controls;
using Dock.Model.Core;
using Kaigara.ViewModels;
using ReactiveUI;
using System.Reactive.Linq;
using System.Diagnostics;
using Kaigara.Collections.ObjectModel;
using System.Reflection;
using Dock.Model.ReactiveUI.Controls;

namespace Kaigara.Shell.ViewModels
{
    public class ShellViewModel : ViewModel, IShell
    {
        private IFactory factory;
        private readonly ILifetimeScope lifetimeScope;

        public IFactory Factory
        {
            get => factory;
            private set => this.RaiseAndSetIfChanged(ref factory, value);
        }

        private IRootDock layout;

        public IRootDock Layout
        {
            get => layout;
            private set => this.RaiseAndSetIfChanged(ref layout, value);
        }
                
        public DockableCollection<Document> Documents { get; }
        public DockableCollection<Tool> Tools { get; }


        public ShellViewModel(IFactory factory, ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            layout = factory.CreateLayout()!;
            factory.InitLayout(layout);

            Documents = new DockableCollection<Document>(factory, layout, lifetimeScope.Resolve, GetDocumentsDock);
            Tools = new DockableCollection<Tool>(factory, layout, lifetimeScope.Resolve, GetToolsDock);

         

            Documents.Active.Subscribe(d =>
            {
                Debug.WriteLine($"Active {d?.Id ?? "NULL"} {d?.Title ?? "NULL"}");
            });

            //fo.ActiveDockableChanged.Select(e => e.EventArgs.Dockable).Subscribe(d =>
            //{
            //    Debug.WriteLine($"Active {d?.Id ?? "NULL"}");
            //});

            //fo.FocusedDockableChanged.Select(e => e.EventArgs.Dockable).Subscribe(d =>
            //{
            //    Debug.WriteLine($"Focus {d?.Id ?? "NULL"}");
            //});

            //fo.DockableClosed.Select(e => e.EventArgs.Dockable).Subscribe(d =>
            //{
            //    Debug.WriteLine($"Closed {d?.Id ?? "NULL"}");
            //});

            //fo.WindowRemoved.Select(e => e.EventArgs.Window).Subscribe(d =>
            //{
            //    Debug.WriteLine($"Window Removed {d?.Id ?? "NULL"}");
            //});
        }

        private IDocumentDock GetDocumentsDock(Document document)
        {
            return factory.GetDockable<IDocumentDock>(ShellDockIds.Documents)!;
        }

        private IToolDock GetToolsDock(Tool tool)
        {
            var dockId = tool.GetType().GetCustomAttribute<ToolAttribute>()?.DefaultDockId ?? ShellDockIds.DefaultToolDock;

            return factory.GetDockable<IToolDock>(dockId)!;
        }
    }
}