using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Shell.ViewModels;

namespace Kaigara.Shell;

public class ShellDockFactory : Factory
{


    private readonly Func<IHostWindow> hostWindowFactory;
    private IRootDock? rootDock;
    private ProportionalDock? mainLayout;
    private ProportionalDock? mainLayoutVertical;
    private IDocumentDock? mainDocumentsDock;
    private IToolDock? leftToolDock;
    private IToolDock? rightToolDock;
    private IToolDock? bottomToolDock;

    private Dictionary<string, IToolDock> toolDockLocator = new Dictionary<string, IToolDock>();

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
        var mainDocumentsDock = CreateDocumentDock();
        mainDocumentsDock.Id = ShellDockIds.Documents;
        mainDocumentsDock.Title = "Documents";
        mainDocumentsDock.IsCollapsable = false;
        mainDocumentsDock.VisibleDockables = CreateList<IDockable>();
        

        var mainLayout = new ProportionalDock
        {
            Id = "MainLayout",
            Title = "MainLayout",
            Orientation = Orientation.Horizontal,
            ActiveDockable = null,
            VisibleDockables = base.CreateList<IDockable>
            (
                mainDocumentsDock
            )
        };

        var mainLayoutVertical = new ProportionalDock
        {
            Id = "MainLayoutVertical",
            Title = "MainLayoutVertical",
            Orientation = Orientation.Vertical,
            ActiveDockable = null,
            VisibleDockables = base.CreateList<IDockable>
            (
                mainLayout
            )
        };

        var rootDock = CreateRootDock();

        rootDock.Id = ShellDockIds.RootDock;
        rootDock.Title = ShellDockIds.RootDock;
        rootDock.ActiveDockable = mainLayoutVertical;
        rootDock.DefaultDockable = mainLayoutVertical;
        rootDock.IsFocusableRoot = true;
        rootDock.VisibleDockables = CreateList<IDockable>(mainLayoutVertical);

        this.rootDock = rootDock;
        this.mainLayout = mainLayout;
        this.mainLayoutVertical = mainLayoutVertical;
        this.mainDocumentsDock = mainDocumentsDock;

        return rootDock;
    }

    public override IRootDock CreateRootDock()
    {
        return new ShellRootDockViewModel();
    }

    public override IToolDock CreateToolDock()
    {
        return base.CreateToolDock();
    }

    public override IDocumentDock CreateDocumentDock()
    {
        return base.CreateDocumentDock();
    }

    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new Dictionary<string, Func<object?>>
        {

        };

        HostWindowLocator = new Dictionary<string, Func<IHostWindow?>>
        {
            [nameof(IDockWindow)] = hostWindowFactory
        };

        DockableLocator = new Dictionary<string, Func<IDockable?>>
        {
            [ShellDockIds.RootDock] = () => rootDock,
            [ShellDockIds.Documents] = () => mainDocumentsDock,

            [ShellDockIds.LeftToolDock] = () => GetLeftToolDock(),
            [ShellDockIds.RightToolDock] = () => GetRightToolDock(),
            [ShellDockIds.DefaultToolDock] = () => GetRightToolDock(),
            [ShellDockIds.BottomToolDock] = () => GetBottomToolDock(),
        };

        base.InitLayout(layout);
    }

    public override void OnDockableRemoved(IDockable? dockable)
    {
        base.OnDockableRemoved(dockable);

        RemoveCached(dockable, ref bottomToolDock, ShellDockIds.BottomToolDock);
        RemoveCached(dockable, ref leftToolDock, ShellDockIds.LeftToolDock);
        RemoveCached(dockable, ref rightToolDock, ShellDockIds.RightToolDock);

        static void RemoveCached(IDockable? dockable, ref IToolDock? dock, string id)
        {
            if (dockable == dock)
            {
                dock = null;

                if (dockable?.Id == id)
                {
                    dockable.Id = Guid.NewGuid().ToString();
                }
            }
        }
    }

    private IToolDock GetLeftToolDock()
    {

        if (leftToolDock is { }) return leftToolDock;

        var (toolDock, dock) = GetToolDock(Alignment.Left, Orientation.Vertical);
        toolDock.Id = ShellDockIds.LeftToolDock;

        InsertDockable(mainLayout!, dock, 0);
        InsertDockable(mainLayout!, new ProportionalDockSplitter(), 1);
        leftToolDock = toolDock;
        dock.Proportion = 0.5;

        return toolDock;
    }

    private IToolDock GetRightToolDock()
    {
        if (rightToolDock is { }) return rightToolDock;

        var (toolDock, dock) = GetToolDock(Alignment.Right, Orientation.Vertical);
        toolDock.Id = ShellDockIds.RightToolDock;

        AddDockable(mainLayout!, new ProportionalDockSplitter());
        AddDockable(mainLayout!, dock);
        rightToolDock = toolDock;
        dock.Proportion = 0.5;
        return toolDock;
    }

    private IToolDock GetBottomToolDock()
    {
        if (bottomToolDock is { }) return bottomToolDock;

        var (toolDock, dock) = GetToolDock(Alignment.Bottom, Orientation.Horizontal);
        toolDock.Id = ShellDockIds.BottomToolDock;

        AddDockable(mainLayoutVertical!, new ProportionalDockSplitter());
        AddDockable(mainLayoutVertical!, dock);
        bottomToolDock = toolDock;

        return toolDock;
    }

    private (IToolDock toolDock, ProportionalDock dock) GetToolDock(Alignment alignment, Orientation orientation)
    {
        var toolDock = CreateToolDock();

        toolDock.ActiveDockable = null;
        toolDock.VisibleDockables = base.CreateList<IDockable>();
        toolDock.Alignment = alignment;
        toolDock.IsCollapsable = true;

        
        var dock = new ProportionalDock
        {
            Proportion = 0.25,
            IsCollapsable = true,
            Orientation = orientation,
            ActiveDockable = null,
            VisibleDockables = base.CreateList<IDockable>
           (
                toolDock
           )
        };

        return (toolDock, dock);
    }
}
