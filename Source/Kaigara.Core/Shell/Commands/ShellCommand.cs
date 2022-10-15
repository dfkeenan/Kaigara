using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Commands;
using Kaigara.Reactive;
using ReactiveUI;

namespace Kaigara.Shell.Commands;
public abstract class ShellCommand : RegisteredCommand
{
    private IShell? shell;
    public IShell? Shell { get => shell; set => this.RaiseAndSetIfChanged(ref shell, value); }

    protected override IObservable<bool>? GetCanExecute()
    {
        return this.WhenAnyValue(x => x.Shell).Select(s => s is not null);
    }
}

public abstract class ShellAsyncCommand : RegisteredAsyncCommand
{
    private IShell? shell;
    public IShell? Shell { get => shell; set => this.RaiseAndSetIfChanged(ref shell, value); }

    protected override IObservable<bool>? GetCanExecute()
    {
        return this.WhenAnyValue(x => x.Shell).Select(s => s is not null);
    }
}

public abstract class ActiveDocumentCommand<TDocument> : ShellCommand
{
    protected override IObservable<bool>? GetCanExecute()
    {
        return this.WhenAnyValue(x => x.Shell)
            .SelectMany(s => s?.Documents.Active!.Is<TDocument>(GetCanExecute) ?? Observable.Return(false));
    }

    protected virtual IObservable<bool> GetCanExecute(TDocument document)
        => Observable.Return(true);
}

public abstract class ActiveDocumentAsyncCommand<TDocument> : ShellAsyncCommand
{
    protected override IObservable<bool>? GetCanExecute()
    {
        return this.WhenAnyValue(x => x.Shell)
            .SelectMany(s => s?.Documents.Active!.Is<TDocument>(GetCanExecute) ?? Observable.Return(false));
    }

    protected virtual IObservable<bool> GetCanExecute(TDocument document)
        => Observable.Return(true);
}
