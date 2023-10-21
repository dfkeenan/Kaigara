using System.Reactive.Linq;
using Dock.Model.ReactiveUI.Controls;
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
        where TDocument : Document
{
    protected override IObservable<bool>? GetCanExecute()
    {
        return this.WhenAnyValue(x => x.Shell)
            .SelectMany(s => s?.Documents.Active!.Is<TDocument>(GetCanExecute) ?? Observable.Return(false));
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
    protected override IObservable<bool>? GetCanExecute()
    {
        return this.WhenAnyValue(x => x.Shell)
            .SelectMany(s => s?.Documents.Active!.Is<TDocument>(GetCanExecute) ?? Observable.Return(false));
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
