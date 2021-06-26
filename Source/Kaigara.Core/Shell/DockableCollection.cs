using System.Collections.ObjectModel;
using System.Linq;
using Dock.Model.Core;
using System.Reactive.Linq;
using Kaigara.Reactive;
using System;
using Dock.Model.Controls;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace Kaigara.Shell
{
    public class DockableCollection<T> : DockableCollectionBase<T>
        where T : IDockable
    {
        private readonly Func<Type, object> dockableFactory;
        private readonly Func<T, IDock> dockLocator;

        public DockableCollection(IFactory factory, IRootDock rootDock, Func<Type, object> dockableFactory, Func<T, IDock> dockLocator)
            : this(factory, rootDock, dockableFactory, dockLocator, new ObservableCollection<T>())
        {

        }

        protected DockableCollection(IFactory factory, IRootDock rootDock, Func<Type, object> dockableFactory, Func<T, IDock> dockLocator, ObservableCollection<T> list)
            : base(factory,rootDock, list)
        {
            this.dockableFactory = dockableFactory ?? throw new ArgumentNullException(nameof(dockableFactory));
            this.dockLocator = dockLocator ?? throw new ArgumentNullException(nameof(dockLocator));
        }

        public void Open(T dockable, bool focus = true)
        {
            if (dockable is null)
            {
                throw new ArgumentNullException(nameof(dockable));
            }

            if (!List.Contains(dockable))
            {
                List.Add(dockable);

                IDock? dock = dockLocator(dockable);
                if (dock is { })
                {
                    Factory.AddDockable(dock, dockable);
                    if (focus)
                    {
                        Factory.SetActiveDockable(dockable);
                        Factory.SetFocusedDockable(dock, dockable); 
                    }
                }
            }
            else if (dockable.Owner is null)
            {
                IDock? dock = dockLocator(dockable);
                if (dock is { })
                {
                    Factory.AddDockable(dock, dockable);
                    if (focus)
                    { 
                        Factory.SetActiveDockable(dockable);
                        Factory.SetFocusedDockable(dock, dockable); 
                    }
                }
            }

            Factory.SetActiveDockable(dockable);
            Factory.SetFocusedDockable(RootDock, dockable);
        }

        public TDockable Open<TDockable>(bool focus = false)
        {
            TDockable dockable = (TDockable)dockableFactory(typeof(TDockable));

            Open((T)(object)dockable, focus);
            return dockable;
        }
    }

    public class ReadOnlyDockableCollection<T> : DockableCollectionBase<T>
        where T : IDockable
    {
        public ReadOnlyDockableCollection(IFactory factory, IRootDock rootDock)
            : this(factory, rootDock, new ObservableCollection<T>())
        {

        }

        public ReadOnlyDockableCollection(IFactory factory, IRootDock rootDock, ObservableCollection<T> list) 
            : base(factory, rootDock, list)
        {
        }
    }
}