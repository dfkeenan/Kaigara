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
    public class DockableCollection<T> : ReadOnlyObservableCollection<T>, IDisposable
        where T : class, IDockable
    {
        private readonly IFactory factory;
        private readonly IRootDock rootDock;
        private readonly Func<Type, object> dockableFactory;
        private readonly Func<T, IDock> dockLocator;
        private readonly ObservableCollection<T> list;
        private CompositeDisposable disposables;

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
            disposables = new CompositeDisposable();

            var fo = new ReactiveDockFactory(factory);
            var activated = fo.FocusedDockableChanged.Select(e => e.EventArgs.Dockable!)
                              .Merge(fo.ActiveDockableChanged.Select(e => e.EventArgs.Dockable!))
                              .OfType<T>()
                              .DistinctUntilChanged();

            var active = new ReadOnlyBehavourSubject<T?>(default(T));
            activated.Subscribe(active).DisposeWith(disposables);
            Active = active;


            fo.DockableClosed.Select(e => e.EventArgs.Dockable!).OfType<T>().Subscribe(e =>
            {

                if(list.Remove(e) && !list.Any() && active.Value is not null)
                {
                    ((IObserver<T?>)active).OnNext(null);
                }
            }).DisposeWith(disposables);
        }

        public IReadOnlyBehavourSubject<T?> Active { get; }

        public void Dispose()
        {
            disposables.Dispose();
        }

        public void Open(T dockable, bool focus = true)
        {
            if (dockable is null)
            {
                throw new ArgumentNullException(nameof(dockable));
            }

            if (!list.Contains(dockable))
            {
                list.Add(dockable);

                IDock? dock = dockLocator(dockable);
                if (dock is { })
                {
                    factory.AddDockable(dock, dockable);
                    if (focus)
                    {
                        factory.SetActiveDockable(dockable);
                        factory.SetFocusedDockable(dock, dockable); 
                    }
                }
            }
            else if (dockable.Owner is null)
            {
                IDock? dock = dockLocator(dockable);
                if (dock is { })
                {
                    factory.AddDockable(dock, dockable);
                    if (focus)
                    { 
                        factory.SetActiveDockable(dockable);
                        factory.SetFocusedDockable(dock, dockable); 
                    }
                }
            }

            factory.SetActiveDockable(dockable);
            factory.SetFocusedDockable(rootDock, dockable);
        }

        public TDockable Open<TDockable>(bool focus = false)
        {
            TDockable dockable = (TDockable)dockableFactory(typeof(TDockable));

            Open((T)(object)dockable, focus);
            return dockable;
        }
    }
}