﻿using System.Windows.Input;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace Kaigara.Commands;

public abstract class RegisteredCommand : RegisteredCommandBase
{
    public RegisteredCommand()
        : base()
    {

    }

    public RegisteredCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected RegisteredCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {

    }

    protected override ICommand CreateCommand()
    {
        return ReactiveCommand.Create(OnExecute, GetCanExecute(), AvaloniaScheduler.Instance);
    }

    protected abstract void OnExecute();
}

public abstract class RegisteredCommand<TParam> : RegisteredCommandBase
{
    public RegisteredCommand()
       : base()
    {

    }

    public RegisteredCommand(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {
    }

    protected RegisteredCommand(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {

    }

    protected override ICommand CreateCommand()
    {
        return ReactiveCommand.Create<TParam>(OnExecute, GetCanExecute(), AvaloniaScheduler.Instance);
    }

    protected abstract void OnExecute(TParam? parameter);

    protected internal override bool CanExecute(object? parameter)
        => CanExecute((TParam?)parameter);

    protected virtual bool CanExecute(TParam? parameter)
    {
        return true;
    }
}