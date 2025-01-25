using Kaigara.Commands;
using Kaigara.StatusBar;
using Kaigara.Toolbars;

namespace ExampleApplication.Commands;


[ToolbarItemDefinition("ProgressExample", "MainToolbarTray/Example")]
[CommandDefinition("Progress Example Command", IconName = "Execute")]
public class ProgressStatusExampleCommand : RegisteredAsyncCommand
{
    private readonly IStatusBar statusBar;

    public ProgressStatusExampleCommand(IStatusBar statusBar)
    {
        this.statusBar = statusBar;
    }

    protected override Task OnExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            for (var i = 0.0; i < 100.0; i += Random.Shared.Next(0, 25))
            {
                i = Math.Clamp(i, 0, 100);
                await Task.Delay(Random.Shared.Next(500, 1000));
                statusBar.UpdateStatus($"Doing stuff {i}", i);
            }

            statusBar.Ready();
        }, cancellationToken);
    }
}
