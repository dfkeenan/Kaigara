using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Collections.ObjectModel;

namespace Kaigara.Commands
{
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

            Commands = this.commands.AsReadOnlyObservableCollection();
        }

        public ReadOnlyObservableCollection<RegisteredCommandBase> Commands { get; }
    }
}
