﻿using System.Reactive.Linq;
using Avalonia.Input;

namespace Kaigara.Commands;

public abstract class ReactiveRegisteredCommandBase : RegisteredCommandBase
{
    public ReactiveRegisteredCommandBase()
        : base()
    {

    }

    public ReactiveRegisteredCommandBase(CommandDefinitionAttribute? attribute)
        : base(attribute)
    {

    }

    protected ReactiveRegisteredCommandBase(string name, string label, KeyGesture? keyGesture, string? iconName)
        : base(name, label, keyGesture, iconName)
    {

    }
    protected virtual IObservable<bool> GetCanExecute() => Observable.Return(true);
}
