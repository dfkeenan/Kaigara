using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Core.Events;
using ReactiveUI;
using System.Reactive;

namespace Kaigara.Shell
{
    internal sealed class ReactiveDockFactory
    {
        public ReactiveDockFactory(IFactory factory)
        {
            this.Factory = factory ?? throw new ArgumentNullException(nameof(factory));

            ActiveDockableChanged = Observable.FromEventPattern<ActiveDockableChangedEventArgs>(e => factory.ActiveDockableChanged += e, e => factory.ActiveDockableChanged -= e);
            FocusedDockableChanged = Observable.FromEventPattern<FocusedDockableChangedEventArgs>(e => factory.FocusedDockableChanged += e, e => factory.FocusedDockableChanged -= e);
            DockableAdded = Observable.FromEventPattern<DockableAddedEventArgs>(e => factory.DockableAdded += e, e => factory.DockableAdded -= e);
            DockableRemoved = Observable.FromEventPattern<DockableRemovedEventArgs>(e => factory.DockableRemoved += e, e => factory.DockableRemoved -= e);
            DockableClosed = Observable.FromEventPattern<DockableClosedEventArgs>(e => factory.DockableClosed += e, e => factory.DockableClosed -= e);
            DockableMoved = Observable.FromEventPattern<DockableMovedEventArgs>(e => factory.DockableMoved += e, e => factory.DockableMoved -= e);
            DockableSwapped = Observable.FromEventPattern<DockableSwappedEventArgs>(e => factory.DockableSwapped += e, e => factory.DockableSwapped -= e);
            DockablePinned = Observable.FromEventPattern<DockablePinnedEventArgs>(e => factory.DockablePinned += e, e => factory.DockablePinned -= e);
            DockableUnpinned = Observable.FromEventPattern<DockableUnpinnedEventArgs>(e => factory.DockableUnpinned += e, e => factory.DockableUnpinned -= e);
            WindowOpened = Observable.FromEventPattern<WindowOpenedEventArgs>(e => factory.WindowOpened += e, e => factory.WindowOpened -= e);
            WindowClosed = Observable.FromEventPattern<WindowClosedEventArgs>(e => factory.WindowClosed += e, e => factory.WindowClosed -= e);
            WindowAdded = Observable.FromEventPattern<WindowAddedEventArgs>(e => factory.WindowAdded += e, e => factory.WindowAdded -= e);
            WindowRemoved = Observable.FromEventPattern<WindowRemovedEventArgs>(e => factory.WindowRemoved += e, e => factory.WindowRemoved -= e);
        }

        public IFactory Factory { get; }

        public IObservable<EventPattern<ActiveDockableChangedEventArgs>> ActiveDockableChanged { get; }
        public IObservable<EventPattern<FocusedDockableChangedEventArgs>> FocusedDockableChanged { get; }
        public IObservable<EventPattern<DockableAddedEventArgs>> DockableAdded { get; }
        public IObservable<EventPattern<DockableRemovedEventArgs>> DockableRemoved { get; }
        public IObservable<EventPattern<DockableClosedEventArgs>> DockableClosed { get; }
        public IObservable<EventPattern<DockableMovedEventArgs>> DockableMoved { get; }
        public IObservable<EventPattern<DockableSwappedEventArgs>> DockableSwapped { get; }
        public IObservable<EventPattern<DockablePinnedEventArgs>> DockablePinned { get; }
        public IObservable<EventPattern<DockableUnpinnedEventArgs>> DockableUnpinned { get; }
        public IObservable<EventPattern<WindowOpenedEventArgs>> WindowOpened { get; }
        public IObservable<EventPattern<WindowClosedEventArgs>> WindowClosed { get; }
        public IObservable<EventPattern<WindowAddedEventArgs>> WindowAdded { get; }
        public IObservable<EventPattern<WindowRemovedEventArgs>> WindowRemoved { get; }
    }
}
