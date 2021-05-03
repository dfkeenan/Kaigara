using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Shell.ViewModels;

namespace Kaigara.Shell
{
    public class ShellDockFactory : Factory
    {
        private readonly Func<IHostWindow> hostWindowFactory;

        public ShellDockFactory(Func<IHostWindow> hostWindowFactory)
        {
            this.hostWindowFactory = hostWindowFactory ?? throw new ArgumentNullException(nameof(hostWindowFactory));
        }
        public override IRootDock CreateLayout()
        {
            var mainLayout = new ProportionalDock
            {
                Id = "MainLayout",
                Title = "MainLayout",
                Proportion = double.NaN,
                Orientation = Orientation.Horizontal,
                ActiveDockable = null,
                VisibleDockables = CreateList<IDockable>
                (
                    new DocumentDock
                    {
                        Id = "DocumentsPane",
                        Title = "DocumentsPane",
                        Proportion = double.NaN,
                        IsCollapsable = false,
                        VisibleDockables = CreateList<IDockable>
                        (
                           
                        )
                    }
                )
            };

            var mainView = new ShellRootDockViewModel
            {
                Id = "Main",
                Title = "Main",
                ActiveDockable = mainLayout,
                VisibleDockables = CreateList<IDockable>(mainLayout)
            };

            var root = CreateRootDock();

            root.Id = "Root";
            root.Title = "Root";
            root.ActiveDockable = mainView;
            root.DefaultDockable = mainView;
            root.VisibleDockables = CreateList<IDockable>(mainView);

            return root;
        }

        public override void InitLayout(IDockable layout)
        {
            this.ContextLocator = new Dictionary<string, Func<object>>
            {
                //[nameof(IRootDock)] = () => _context,
                //// [nameof(IPinDock)] = () => _context,
                //[nameof(IProportionalDock)] = () => _context,
                //[nameof(IDocumentDock)] = () => _context,
                //[nameof(IToolDock)] = () => _context,
                //[nameof(ISplitterDockable)] = () => _context,
                //[nameof(IDockWindow)] = () => _context,
                //[nameof(IDocument)] = () => _context,
                //[nameof(ITool)] = () => _context,
                //["Document1"] = () =>
                //{
                //    return new Document1();
                //},
                //["Document2"] = () => new Document2(),
                //["LeftTop1"] = () => new LeftTopTool1(),
                //["LeftTop2"] = () => new LeftTopTool2(),
                //["LeftBottom1"] = () => new LeftBottomTool1(),
                //["LeftBottom2"] = () => new LeftBottomTool2(),
                //["RightTop1"] = () => new RightTopTool1(),
                //["RightTop2"] = () => new RightTopTool2(),
                //["RightBottom1"] = () => new RightBottomTool1(),
                //["RightBottom2"] = () => new RightBottomTool2(),
                //["LeftPane"] = () => _context,
                //["LeftPaneTop"] = () => _context,
                //["LeftPaneTopSplitter"] = () => _context,
                //["LeftPaneBottom"] = () => _context,
                //["RightPane"] = () => _context,
                //["RightPaneTop"] = () => _context,
                //["RightPaneTopSplitter"] = () => _context,
                //["RightPaneBottom"] = () => _context,
                //["DocumentsPane"] = () => _context,
                //["MainLayout"] = () => _context,
                //["LeftSplitter"] = () => _context,
                //["RightSplitter"] = () => _context,
                //["MainLayout"] = () => _context,
                //["Main"] = () => _context,
            };

            this.HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
            {
                [nameof(IDockWindow)] = hostWindowFactory
            };

            this.DockableLocator = new Dictionary<string, Func<IDockable?>>
            {
            };

            base.InitLayout(layout);
        }
    }
}
