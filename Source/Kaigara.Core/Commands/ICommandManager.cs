using System.Collections.ObjectModel;

namespace Kaigara.Commands;

public interface ICommandManager
{
    ReadOnlyObservableCollection<RegisteredCommandBase> Commands { get; }
}
