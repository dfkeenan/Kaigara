using Avalonia;
using Avalonia.Markup.Xaml;


namespace ExampleApplication;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
