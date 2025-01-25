namespace Kaigara.IO;

public class ConsoleRedirectWriter : RedirectWriter
{
    private TextWriter consoleTextWriter; //keeps Visual Studio console in scope.

    public ConsoleRedirectWriter()
    {
        consoleTextWriter = Console.Out;
        OnWrite += WriteToStandardConsole;
        Console.SetOut(this);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        OnWrite -= WriteToStandardConsole;
        Console.SetOut(consoleTextWriter);
    }

    private void WriteToStandardConsole(string? text)
    {
        consoleTextWriter.Write(text);
    }
}
