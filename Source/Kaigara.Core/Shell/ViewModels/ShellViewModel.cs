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
                
        public DockableCollection<IDocument> Documents { get; }
        public DockableCollection<ITool> Tools { get; }


        public ShellViewModel(IFactory factory, ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            layout = factory.CreateLayout()!;
            factory.InitLayout(layout);

            Documents = new DockableCollection<IDocument>(factory, layout, lifetimeScope.Resolve, GetDocumentsDock);
            Tools = new DockableCollection<ITool>(factory, layout, lifetimeScope.Resolve, GetToolsDock);

         

            Documents.Active.Subscribe(d =>
            {
                Debug.WriteLine($"Active {d?.Id ?? "NULL"}");
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

        private IDocumentDock GetDocumentsDock(IDocument document)
        {
            return factory.GetDockable<IDocumentDock>("Documents")!;
        }

        private IToolDock GetToolsDock(ITool tool)
        {
            throw new NotImplementedException();
        }
    }
}