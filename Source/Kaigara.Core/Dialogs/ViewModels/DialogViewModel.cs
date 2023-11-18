using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.ViewModels;
using ReactiveUI;

namespace Kaigara.Dialogs.ViewModels;
public abstract class DialogViewModel : WindowViewModel, IDialogViewModel
{
    internal Interaction<object, Unit> CloseInteraction { get; } = new Interaction<object, Unit>();
    public async Task Close()
    {
        await CloseInteraction.Handle(Unit.Default);
    }
}

public abstract class DialogViewModel<TResult> : DialogViewModel, IDialogViewModel<TResult>
{
    public async Task Close(TResult result)
    {
        await CloseInteraction.Handle(result!);
    }
}
