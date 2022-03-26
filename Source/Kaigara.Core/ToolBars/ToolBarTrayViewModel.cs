﻿using System.Collections.ObjectModel;
using Kaigara.Collections.ObjectModel;

namespace Kaigara.ToolBars;

public class ToolBarTrayViewModel : IDisposable
{
    private readonly ToolBarTrayDefinition definition;
    private ReadOnlyObservableCollection<IToolBarViewModel> items;

    public ToolBarTrayViewModel()
    {
        this.definition = CreateDefinition();
        items = definition.Items.ToReadOnlyObservableCollectionOf(d => d.Build());
    }

    public ToolBarTrayViewModel(ToolBarTrayDefinition definition)
    {
        this.definition = definition ?? throw new ArgumentNullException(nameof(definition));
        items = definition.Items.ToReadOnlyObservableCollectionOf(d => d.Build());
    }

    public IEnumerable<IToolBarViewModel> Items => items;

    public ToolBarTrayDefinition Definition => definition;

    protected virtual ToolBarTrayDefinition CreateDefinition()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        foreach (var item in items)
        {
            item.Dispose();
        }
    }
}