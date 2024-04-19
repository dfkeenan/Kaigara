using Kaigara.Configuration;
using Kaigara.Configuration.UI;
using Kaigara.Configuration.UI.ViewModels;
using ReactiveUI;

namespace ExampleApplication.Configuration;




public record class ExampleCategory()
    : OptionCategory<EnvironmentCategory>("Example Category");


public record class ExampleConfig : IOptionsModel
{
    public string Value { get; init; }
}

[OptionsPage<ExampleConfig, ExampleCategory>("Example")]

public class ExampleConfigPageViewModel : OptionsPageViewModel
{
    private string value;

    public string Value
    {
        get => value;
        set => this.RaiseAndSetIfChanged(ref this.value, value);
    }
}


//public record class AnotherExampleConfig : IOptionsModel
//{
//    public string Value { get; init; }
//}

//[OptionsPage<AnotherExampleConfig, ExampleCategory>("Another Example")]
//public class AnotherExampleConfigPageViewModel : OptionsPageViewModel
//{

//}
