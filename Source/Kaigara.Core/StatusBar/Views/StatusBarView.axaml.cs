using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Kaigara.Collections.ObjectModel;
using Kaigara.StatusBar.ViewModels;
using ReactiveUI;

namespace Kaigara.StatusBar.Views;
public partial class StatusBarView : ReactiveUserControl<StatusBarViewModel>
{
    public StatusBarView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            if (ViewModel is not null && StatusItemsControl.ItemsPanelRoot is Grid itemsPanelGrid)
            {
                itemsPanelGrid.ColumnDefinitions.Clear();
                ViewModel.Items
                    .SyncTo(itemsPanelGrid.ColumnDefinitions, (StatusBarItemViewModel i) => new ColumnDefinition(i.Width))
                    .DisposeWith(d);
            }
        });
    }
}
