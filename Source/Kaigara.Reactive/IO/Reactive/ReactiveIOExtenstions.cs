
using System.Reactive.Linq;

namespace Kaigara.IO.Reactive;

public static class ReactiveIOExtenstions
{
    public static IObservable<FileSystemEventArgs> FromChangedEvent(this FileSystemWatcher fileSystemWatcher)
    {
        return Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    h => fileSystemWatcher.Changed += h,
                    h => fileSystemWatcher.Changed -= h)
                    .Select(x => x.EventArgs);
    }

    public static IObservable<string> FromWriteEvent(this RedirectWriter redirectWriter)
    {
        return Observable.FromEvent<Action<String>, string>(
                    h => redirectWriter.OnWrite += h,
                    h => redirectWriter.OnWrite -= h);
    }
}
