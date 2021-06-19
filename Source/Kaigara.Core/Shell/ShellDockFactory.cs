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
            var mainDocumentsDock = new DocumentDock
            {
                Id = ShellDockIds.Documents,
                Title = "Documents",
                IsCollapsable = false,
                VisibleDockables = CreateList<IDockable>()
            };

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

        public override void InitLayout(IDockable layout)
        {
            ContextLocator = new Dictionary<string, Func<object>>
            {
                
            };

            HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
            {
                [nameof(IDockWindow)] = hostWindowFactory
            };

            DockableLocator = new Dictionary<string, Func<IDockable?>>
            {
                [ShellDockIds.RootDock] = () => rootDock,
                [ShellDockIds.Documents] = ()=> mainDocumentsDock,

                [ShellDockIds.LeftToolDock] = ()=> GetLeftToolDock(),
                [ShellDockIds.RightToolDock] = ()=> GetRightToolDock(),
                [ShellDockIds.DefaultToolDock] = ()=> GetRightToolDock(),
                [ShellDockIds.BottomToolDock] = ()=> GetBottomToolDock(),
            };

            base.InitLayout(layout);
        }

        private IToolDock GetLeftToolDock()
        {
            if (leftToolDock is { }) return leftToolDock;

            var toolDock = new ToolDock()
            {
                Id = ShellDockIds.LeftToolDock,
                ActiveDockable = null,
                VisibleDockables = CreateList<IDockable>(),
                Alignment = Alignment.Left,
                IsCollapsable = true,
                
            };

            var leftDock = new ProportionalDock
            {
                Proportion = 0.25,
                IsCollapsable = true,
                Orientation = Orientation.Vertical,
                ActiveDockable = null,
                VisibleDockables = CreateList<IDockable>
               (
                    toolDock
               )
            };

            InsertDockable(mainLayout!, leftDock,0);
            InsertDockable(mainLayout!, new SplitterDockable(), 1);

            leftToolDock = toolDock;
            return toolDock;
        }

        private IToolDock GetRightToolDock()
        {
            if (rightToolDock is { }) return rightToolDock;

            var toolDock = new ToolDock()
            {
                Id = ShellDockIds.RightToolDock,
                ActiveDockable = null,
                VisibleDockables = CreateList<IDockable>(),
                Alignment = Alignment.Right,
                IsCollapsable = true,
            };

            var rightDock = new ProportionalDock
            {
                Proportion = 0.25,
                Orientation = Orientation.Vertical,
                ActiveDockable = null,
                VisibleDockables = CreateList<IDockable>
               (
                    toolDock
               )
            };

            AddDockable(mainLayout!, new SplitterDockable());
            AddDockable(mainLayout!, rightDock);
            rightToolDock = toolDock;
            return toolDock;
        }

        private IToolDock GetBottomToolDock()
        {
            if (bottomToolDock is { }) return bottomToolDock;

            var toolDock = new ToolDock()
            {
                Id = ShellDockIds.BottomToolDock,
                ActiveDockable = null,
                VisibleDockables = CreateList<IDockable>(),
                Alignment = Alignment.Bottom,
                IsCollapsable = true,
            };

            var bottomDock = new ProportionalDock
            {
                Proportion = 0.25,
                Orientation = Orientation.Horizontal,
                ActiveDockable = null,
                Owner = mainLayoutVertical,
                Factory = this,
                VisibleDockables = CreateList<IDockable>
               (
                    toolDock
               )
            };

            AddDockable(mainLayoutVertical!, new SplitterDockable());
            AddDockable(mainLayoutVertical!, bottomDock);

            bottomToolDock = toolDock;
            return toolDock;
        }
    }
}
