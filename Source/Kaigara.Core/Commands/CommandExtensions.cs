using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kaigara.Commands;
public static class CommandExtensions
{
    //public static ICommand WrapCommand<TParam>(this ICommand command, Func<TParam, bool>? canExecuteWrapper = null, Action<ICommand, TParam>? executeWrapper = null)
    //    => new CommandWrapper<TParam>(command, canExecuteWrapper, executeWrapper);

    public static ICommand OverrideCanExecute<TParam>(this ICommand command, Func<TParam, bool>? canExecute)
        => new CommandWrapper<TParam>(command, canExecute);

}

internal class CommandWrapper<TParam> : ICommand
{
    readonly ICommand command;
    private readonly Func<TParam, bool>? canExecute;
    private readonly Action<ICommand, TParam>? execute;

    public CommandWrapper(ICommand command, Func<TParam, bool>? canExecute = null, Action<ICommand, TParam>? execute = null)
    {
        this.command = command;
        this.canExecute = canExecute;
        this.execute = execute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add
        {
            command.CanExecuteChanged += value;
        }

        remove
        {
            command.CanExecuteChanged -= value;
        }
    }

    public bool CanExecute(object? parameter)
    {
        if(canExecute is null)
        {
            return command.CanExecute(parameter);
        }

        return canExecute.Invoke(parameter is TParam p ? p : default!) && command.CanExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        if(execute is null)
        {
            command.Execute(parameter);
            return;
        }

        execute.Invoke(command, parameter is TParam p ? p : default!);
    }
}
