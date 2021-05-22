using System.Collections.Generic;

namespace Kaigara.Menus
{
    public interface IMenuItem
    {
        string Name { get; }
        ICollection<IMenuItem>? Items { get; }
    }
}