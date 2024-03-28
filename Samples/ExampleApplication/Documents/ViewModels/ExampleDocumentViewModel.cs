using Kaigara.Configuration;
using Kaigara.Menus;
using Kaigara.Shell.ViewModels;
using Microsoft.Extensions.Options;
using ReactiveUI;

namespace ExampleApplication.Documents.ViewModels;

public record class ExampleConfig : IOptionsModel
{
    public string Value { get; init; }
}


public class ExampleDocumentViewModel : ActivatableDocument
{
    public ExampleDocumentViewModel(IMenuManager menuManager, IOptionsMonitor<ExampleConfig> configMonitor)
    {
        Id = Guid.NewGuid().ToString();
        Title = "Example Document";

        configProperty = configMonitor.AsObservable().ToProperty(this, d => d.Config, configMonitor.CurrentValue);
    }

    private bool ischecked;
    private readonly ObservableAsPropertyHelper<ExampleConfig> configProperty;

    public bool IsChecked
    {
        get { return ischecked; }
        set { this.RaiseAndSetIfChanged(ref ischecked, value); }
    }

    public ExampleConfig Config { get => configProperty.Value; }

    protected override void OnDispose()
    {
        base.OnDispose();
        configProperty.Dispose();
    }
}
