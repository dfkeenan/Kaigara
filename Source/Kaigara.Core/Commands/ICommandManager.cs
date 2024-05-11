using System.Collections.ObjectModel;

namespace Kaigara.Commands;

public interface ICommandManager
{
    ReadOnlyObservableCollection<RegisteredCommandBase> Commands { get; }
    IObservable<EventArgs> RequerySuggested { get; }

    event EventHandler<EventArgs> RequerySuggestedEvent;

    void RegisterCommand(RegisteredCommandBase command);
}
