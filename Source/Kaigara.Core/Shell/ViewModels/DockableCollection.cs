using System.Collections.ObjectModel;
using System.Linq;
using Dock.Model.Core;
using System.Reactive.Linq;
using Kaigara.Reactive;
using System;
using Dock.Model.Controls;

namespace Kaigara.Shell.ViewModels
{
    public class DockableCollection<T> : ReadOnlyObservableCollection<T>
        where T : IDockable
    {
        private readonly IFactory factory;
        private readonly IRootDock rootDock;
        private readonly Func<Type, object> dockableFactory;
        private readonly Func<T, IDock> dockLocator;
        private readonly ObservableCollection<T> list;

        public DockableCollection(IFactory factory, IRootDock rootDock, Func<Type, object> dockableFactory, Func<T, IDock> dockLocator)
            : this(factory, rootDock, dockableFactory, dockLocator, new ObservableCollection<T>())
        {

        }

        protected DockableCollection(IFactory factory, IRootDock rootDock, Func<Type, object> dockableFactory, Func<T, IDock> dockLocator, ObservableCollection<T> list)
            : base(list)
        {
            
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.rootDock = rootDock ?? throw new ArgumentNullException(nameof(rootDock));
            this.dockableFactory = dockableFactory ?? throw new ArgumentNullException(nameof(dockableFactory));
            this.dockLocator = dockLocator ?? throw new ArgumentNullException(nameof(dockLocator));
            this.list = list ?? throw new ArgumentNullException(nameof(list));

            var fo = new ReactiveDockFactory(factory);
            var activated = fo.FocusedDockableChanged.Select(e => e.EventArgs.Dockable!)
                              .Merge(fo.ActiveDockableChanged.Select(e => e.EventArgs.Dockable!))
                              .DistinctUntilChanged()
                              .OfType<T?>();

            var active = new ReadOnlyBehavourSubject<T?>(default(T));
            activated.Subscribe(active);
            Active = active;
        }

        public IReadOnlyBehavourSubject<T?> Active { get; }

        public void Open(T dockable, bool focus = false)
        {
            if (dockable is null)
            {
                throw new ArgumentNullException(nameof(dockable));
            }

            if (!list.Contains(dockable))
            {
                list.Add(dockable);

                IDock dock = dockLocator(dockable);
                if (dock is { } && rootDock is { })
                {
                    factory?.AddDockable(dock, dockable);
                    factory?.SetActiveDockable(dockable);
                    factory?.SetFocusedDockable(rootDock, dockable);
                }
                return;
            }

            if (dockable.Owner is null)
            {
                IDock dock = dockLocator(dockable);
                if (dock is { } && rootDock is { })
                {
                    factory?.AddDockable(dock, dockable);
                }
            }

            if (rootDock is { })
            {
                factory?.SetActiveDockable(dockable);
                factory?.SetFocusedDockable(rootDock, dockable);
            }
        }

        public TDockable Open<TDockable>(bool focus = false)
            where TDockable : T
        {
            TDockable dockable = (TDockable)dockableFactory(typeof(TDockable));
            Open(dockable, focus);
            return dockable;
        }
    }
}