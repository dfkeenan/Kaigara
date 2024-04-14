using Kaigara.Configuration;
using Kaigara.Configuration.UI;
using Kaigara.Configuration.UI.ViewModels;

namespace ExampleApplication.Configuration;




public record class ExampleCategory()
    : OptionCategory<EnvironmentCategory>("Example Category");


public record class ExampleConfig : IOptionsModel
{
    public string Value { get; init; }
}

[OptionsPage<ExampleConfig, EnvironmentCategory>("Example")]

public class ExampleConfigPageViewModel : OptionsPageViewModel
{

}


public record class AnotherExampleConfig : IOptionsModel
{
    public string Value { get; init; }
}

[OptionsPage<AnotherExampleConfig, ExampleCategory>("Another Example")]
public class AnotherExampleConfigPageViewModel : OptionsPageViewModel
{

}
