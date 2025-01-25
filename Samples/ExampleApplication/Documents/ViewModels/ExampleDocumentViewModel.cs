using System.Reactive.Disposables;
using ExampleApplication.Configuration;
using Kaigara.Configuration;
using Kaigara.Menus;
using Kaigara.Shell.ViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;

namespace ExampleApplication.Documents.ViewModels;


public class ExampleDocumentViewModel : ActivatableDocument
{
    public ExampleDocumentViewModel(IMenuManager menuManager, IOptionsMonitor<ExampleConfig> configMonitor)
    {
        Id = Guid.NewGuid().ToString();
        Title = "Example Document";
        this.configMonitor = configMonitor;
    }

    private bool ischecked;
    private ObservableAsPropertyHelper<ExampleConfig>? configProperty;
    private readonly IOptionsMonitor<ExampleConfig> configMonitor;

    public bool IsChecked
    {
        get { return ischecked; }
        set { this.RaiseAndSetIfChanged(ref ischecked, value); }
    }

    public ExampleConfig? Config { get => configProperty?.Value; }

    protected override void OnActivated(CompositeDisposable disposables)
    {
        base.OnActivated(disposables);
        configProperty ??= configMonitor.AsObservable().ToProperty(this, d => d.Config, configMonitor.CurrentValue)!;
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        configProperty?.Dispose();
        configProperty = null;
    }
}
