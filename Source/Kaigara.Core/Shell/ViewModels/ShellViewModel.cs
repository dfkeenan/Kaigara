﻿using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reflection;
using Autofac;
using Avalonia.Platform.Storage;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Core.Events;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Collections.ObjectModel;
using Kaigara.ViewModels;
using ReactiveUI;

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
    public IStorageProvider? StorageProvider { get; set; }

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

        //void Debug(IDockable? dockable)
        //{
        //    System.Diagnostics.Debug.WriteLine($"Active Document {Documents.Active.Value?.Title ?? "NULL"}");
        //    System.Diagnostics.Debug.WriteLine($"Active Tool {Tools.Active.Value?.Title ?? "NULL"}");
        //    System.Diagnostics.Debug.WriteLine($"Active Dockable {Dockables.Active.Value?.Title ?? "NULL"}");
        //    System.Diagnostics.Debug.WriteLine($"--------------------------------------------------------");
        //}

        //Dockables.Active.Subscribe(Debug);
        //Documents.Active.Subscribe(Debug);
        //Tools.Active.Subscribe(Debug);

        //var fo = new ReactiveDockFactory(factory);

        //fo.ActiveDockableChanged.Select(e => e.EventArgs.Dockable).Subscribe(d =>
        //{
        //    System.Diagnostics.Debug.WriteLine($"Active {d?.Id ?? "NULL"}");
        //});

        //fo.FocusedDockableChanged.Select(e => e.EventArgs.Dockable).Subscribe(d =>
        //{
        //    System.Diagnostics.Debug.WriteLine($"Focus {d?.Id ?? "NULL"}");
        //});

        //fo.DockableClosed.Select(e => e.EventArgs.Dockable).Subscribe(d =>
        //{
        //    System.Diagnostics.Debug.WriteLine($"Closed {d?.Id ?? "NULL"}");
        //});

        //fo.WindowRemoved.Select(e => e.EventArgs.Window).Subscribe(d =>
        //{
        //    System.Diagnostics.Debug.WriteLine($"Window Removed {d?.Id ?? "NULL"}");
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

    private static IEnumerable<IDockable> GetDocumentsAndTools(IDockable dockable)
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
