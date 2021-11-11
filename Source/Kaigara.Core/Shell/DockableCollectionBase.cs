using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Dock.Model.Controls;
using Dock.Model.Core;
using Kaigara.Reactive;

namespace Kaigara.Shell
{
    public abstract class DockableCollectionBase<T> : ReadOnlyObservableCollection<T>, IDisposable
        where T : IDockable
    {
        protected DockableCollectionBase(IFactory factory, IRootDock rootDock, ObservableCollection<T> list)
          : base(list)
        {

            this.ReactiveFactory = new ReactiveDockFactory(factory ?? throw new ArgumentNullException(nameof(factory)));
            this.RootDock = rootDock ?? throw new ArgumentNullException(nameof(rootDock));
            this.List = list ?? throw new ArgumentNullException(nameof(list));
            Disposables = new CompositeDisposable();

            var activated = this.ReactiveFactory.FocusedDockableChanged.Select(e => e.EventArgs.Dockable!)
                              .Merge(this.ReactiveFactory.ActiveDockableChanged.Select(e => e.EventArgs.Dockable!))
                              .OfType<T>()
                              .DistinctUntilChanged();

            var active = new ReadOnlyBehavourSubject<T?>(default(T));
            activated.Subscribe(active).DisposeWith(Disposables);
            Active = active;
        }

        public IReadOnlyBehavourSubject<T?> Active { get; }

        protected ObservableCollection<T> List { get; }

        internal ReactiveDockFactory ReactiveFactory { get; }

        protected IFactory Factory => ReactiveFactory.Factory;

        protected IRootDock RootDock { get; }
        protected CompositeDisposable Disposables { get; set; }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            Disposables.Dispose();
        }
    }
}