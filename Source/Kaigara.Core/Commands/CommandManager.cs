using System.Collections.ObjectModel;

namespace Kaigara.Commands;

public class CommandManager : ICommandManager
{
    private readonly ObservableCollection<RegisteredCommandBase> commands;

    public CommandManager(IEnumerable<RegisteredCommandBase> commands)
    {
        if (commands is null)
        {
            throw new ArgumentNullException(nameof(commands));
        }

        this.commands = new ObservableCollection<RegisteredCommandBase>(commands);

        Commands = Collections.ObjectModel.ObservableCollectionExtensions.AsReadOnlyObservableCollection<RegisteredCommandBase>(this.commands);
    }

    public ReadOnlyObservableCollection<RegisteredCommandBase> Commands { get; }
}
