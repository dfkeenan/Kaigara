using System.Collections.ObjectModel;
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
using System.Collections.Specialized;
using Dock.Model.Core.Events;

namespace Kaigara.Shell.ViewModels;

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
    public ReadOnlyDockableCollection<IDockable> Dockables { get; }

    public ShellViewModel(IFactory factory, ILifetimeScope lifetimeScope)
    {
        this.lifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope));
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        layout = factory.CreateLayout()!;
        factory.InitLayout(layout);

        Documents = new DockableCollection<Document>(factory, layout, lifetimeScope.Resolve, GetDocumentsDock);
        Tools = new DockableCollection<Tool>(factory, layout, lifetimeScope.Resolve, GetToolsDock);


        var dockables = new ObservableCollection<IDockable>();
        Documents.SyncItemsTo(dockables);
        Tools.SyncItemsTo(dockables);
        Dockables = new ReadOnlyDockableCollection<IDockable>(factory, layout, dockables);        

        factory.WindowClosed += OnDockWindowClosed;

        factory.WindowClosing += OnDockWindowClosing;



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
    private void OnDockWindowClosing(object? sender, WindowClosingEventArgs e)
    {
        var dockables = GetDocumentsAndTools(e.Window!.Layout!).ToList();
        //TODO: Prevent closing of unsaved documents.
        foreach (var item in dockables)
        {

        }

        //e.Cancel = true;
    }

    private void OnDockWindowClosed(object? sender, WindowClosedEventArgs e)
    {
        var dockables = GetDocumentsAndTools(e.Window!.Layout!).ToList();

        foreach (var item in dockables)
        {
            if (item is Tool tool)
            {
                Tools.Remove(tool);
            }
            else if (item is Document document)
            {
                Documents.Remove(document);
            }
        }
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



    static IEnumerable<IDockable> GetDocumentsAndTools(IDockable dockable)
    {

        if (dockable is IDocument || dockable is ITool)
        {
            yield return dockable;
        }
        else if (dockable is IDock dock)
        {
            var dockables = Enumerable.Empty<IDockable>();

            if (dock.VisibleDockables is { })
            {
                dockables = dockables.Concat(dock.VisibleDockables);
            }

            if (dock.PinnedDockables is { })
            {
                dockables = dockables.Concat(dock.PinnedDockables);
            }

            if (dock.HiddenDockables is { })
            {
                dockables = dockables.Concat(dock.HiddenDockables);
            }

            foreach (var item in dockables)
            {
                foreach (var child in GetDocumentsAndTools(item))
                {
                    yield return child;
                }
            }
        }
    }
}
