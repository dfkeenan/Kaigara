using System;
using System.Collections.Generic;

namespace Kaigara.ToolBars
{
    public interface IToolBarViewModel : IDisposable
    {
        string Name { get; }

        bool IsVisible { get; }

        IEnumerable<IToolBarItemViewModel> Items { get; }
    }
}
