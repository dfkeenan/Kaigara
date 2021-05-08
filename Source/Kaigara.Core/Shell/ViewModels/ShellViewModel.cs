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

        private readonly ObservableCollection<IDocument> documents;

        public ReadOnlyObservableCollection<IDocument> Documents { get; }

        private readonly ISubject<IDocument?> documentActivated;

        public IObservable<IDocument?> DocumentActivated => documentActivated;

        private ObservableAsPropertyHelper<ITool?> activeTool;

        public ITool? ActiveTool => activeTool.Value;


        private readonly ObservableCollection<ITool> tools;

        public ReadOnlyObservableCollection<ITool> Tools { get; }

        private readonly ISubject<ITool?> toolActivated;

        public IObservable<ITool?> ToolActivated => toolActivated;

        public ShellViewModel(IFactory factory, ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            layout = factory.CreateLayout()!;
            factory.InitLayout(layout);

            documents = new ObservableCollection<IDocument>();
            Documents = new ReadOnlyObservableCollection<IDocument>(documents);
            documentActivated = new BehaviorSubject<IDocument?>(null);
            activeDocument = documentActivated.ToProperty(this, o => o.ActiveDocument);

            tools = new ObservableCollection<ITool>();
            Tools = new ReadOnlyObservableCollection<ITool>(tools);
            toolActivated = new BehaviorSubject<ITool?>(null);
            activeTool = toolActivated.ToProperty(this, o => o.ActiveTool);
            
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

        public void OpenDocument<TDocument>()
            where TDocument : IDocument
        {
            OpenDocument(lifetimeScope.Resolve<TDocument>());
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

        public void OpenTool<TTool>(bool focus = false)
            where TTool : ITool
        {
            OpenTool(lifetimeScope.Resolve<TTool>(), focus);
        }

        private IToolDock GetToolsDockable(ITool tool)
        {
            throw new NotImplementedException();
        }
    }
}
