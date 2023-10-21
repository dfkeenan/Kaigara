namespace Kaigara.Toolbars;

public interface IToolbarViewModel : IDisposable
{
    string Name { get; }

    bool IsVisible { get; }

    IEnumerable<IToolbarItemViewModel> Items { get; }
}
