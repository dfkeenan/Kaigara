using System.Reactive.Linq;
using Dock.Model.ReactiveUI.Controls;
using Kaigara.Commands;
using Kaigara.Reactive;

namespace Kaigara.Shell.Commands;
public abstract class ShellCommand : RegisteredCommand
{
    public required IShell Shell { get; init; }
}

public abstract class ShellAsyncCommand : RegisteredAsyncCommand
{
    public required IShell Shell { get; init; }
}

public abstract class ActiveDocumentCommand<TDocument> : ShellCommand
        where TDocument : Document
{
    protected override IObservable<bool> GetCanExecute()
    {
        return Shell.Documents.Active.Is<TDocument>(GetCanExecute);
    }

    protected virtual IObservable<bool> GetCanExecute(TDocument document)
        => Observable.Return(true);

    protected override void OnExecute()
    {
        OnExecute((TDocument)Shell!.Documents.Active.Value!);
    }

    protected virtual void OnExecute(TDocument document)
    {

    }
}

public abstract class ActiveDocumentAsyncCommand<TDocument> : ShellAsyncCommand
        where TDocument : Document
{
    protected override IObservable<bool> GetCanExecute()
    {
        return Shell.Documents.Active.Is<TDocument>(GetCanExecute);
    }

    protected virtual IObservable<bool> GetCanExecute(TDocument document)
        => Observable.Return(true);

    protected override Task OnExecuteAsync()
    {
        return OnExecuteAsync((TDocument)Shell!.Documents.Active.Value!);
    }

    protected virtual Task OnExecuteAsync(TDocument document)
    {
        return Task.CompletedTask;
    }
}
