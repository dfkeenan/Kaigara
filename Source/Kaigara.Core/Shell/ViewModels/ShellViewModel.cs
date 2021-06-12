using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Subjects;
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

        private ObservableAsPropertyHelper<IDocument?> activeDocument;

        public IDocument? ActiveDocument => activeDocument.Value;
        public IObservable<IDocument?> DocumentActivated { get; }

        private readonly ObservableCollection<IDocument> documents;

        public ReadOnlyObservableCollection<IDocument> Documents { get; }

        private ObservableAsPropertyHelper<ITool?> activeTool;

        public ITool? ActiveTool => activeTool.Value;

        public IObservable<ITool?> ToolActivated { get; }

        private readonly ObservableCollection<ITool> tools;

        public ReadOnlyObservableCollection<ITool> Tools { get; }


        public ShellViewModel(IFactory factory, ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            layout = factory.CreateLayout()!;
            factory.InitLayout(layout);

            documents = new ObservableCollection<IDocument>();
            Documents = documents.AsReadOnlyObservableCollection();

            tools = new ObservableCollection<ITool>();
            Tools = tools.AsReadOnlyObservableCollection();


            var fo = new ReactiveDockFactory(factory);
            DocumentActivated = fo.FocusedDockableChanged.Select(e => e.EventArgs.Dockable!)
                                .Merge(fo.ActiveDockableChanged.Select(e => e.EventArgs.Dockable!))
                                .DistinctUntilChanged()
                                .OfType<IDocument?>();

            activeDocument = DocumentActivated.ToProperty(this, nameof(ActiveDocument));

            ToolActivated = fo.FocusedDockableChanged.Select(e => e.EventArgs.Dockable!)
                              .Merge(fo.ActiveDockableChanged.Select(e => e.EventArgs.Dockable!))
                              .DistinctUntilChanged()
                              .OfType<ITool?>();

            activeTool = ToolActivated.ToProperty(this, nameof(ActiveTool));

            DocumentActivated.Subscribe(d =>
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

        public void OpenDocument(IDocument document)
        {
            if (document is null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (!documents.Contains(document))
            {
                documents.Add(document);

                var dockable = GetDocumentsDockable();
                if (dockable is { } && layout is { })
                {
                    factory?.AddDockable(dockable, document);
                    factory?.SetActiveDockable(document);
                    factory?.SetFocusedDockable(Layout, document);
                }
                return;
            }

            if (document.Owner is null)
            {
                var dockable = GetDocumentsDockable();
                if (dockable is { } && layout is { })
                {
                    factory?.AddDockable(dockable, document);
                }
            }

            if (layout is { })
            {
                factory?.SetActiveDockable(document);
                factory?.SetFocusedDockable(Layout, document);
            }
        }

        public TDocument OpenDocument<TDocument>()
            where TDocument : IDocument
        {
            TDocument document = lifetimeScope.Resolve<TDocument>();
            OpenDocument(document);

            return document;
        }

        private IDocumentDock? GetDocumentsDockable()
        {
            return factory?.GetDockable<IDocumentDock>("Documents");
        }

        public void OpenTool(ITool tool, bool focus = false)
        {
            if (tool is null)
            {
                throw new ArgumentNullException(nameof(tool));
            }

            if (!tools.Contains(tool))
            {
                tools.Add(tool);

                var dockable = GetToolsDockable(tool);
                if (dockable is { } && layout is { })
                {
                    factory?.AddDockable(dockable, tool);
                    factory?.SetActiveDockable(tool);
                    factory?.SetFocusedDockable(Layout, tool);
                }
                return;
            }

            if (tool.Owner is null)
            {
                var dockable = GetToolsDockable(tool);
                if (dockable is { } && layout is { })
                {
                    factory?.AddDockable(dockable, tool);
                }
            }

            if (layout is { })
            {
                factory?.SetActiveDockable(tool);
                factory?.SetFocusedDockable(Layout, tool);
            }
        }

        public TTool OpenTool<TTool>(bool focus = false)
            where TTool : ITool
        {
            TTool tool = lifetimeScope.Resolve<TTool>();
            OpenTool(tool, focus);
            return tool;
        }

        private IToolDock GetToolsDockable(ITool tool)
        {
            throw new NotImplementedException();
        }
    }
}
