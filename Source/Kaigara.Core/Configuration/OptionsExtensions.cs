using System.Reactive;
using Microsoft.Extensions.Options;

namespace Kaigara.Configuration;
public static class OptionsExtensions
{
    public static IObservable<T> AsObservable<T>(this IOptionsMonitor<T> monitor)
    {
        if (monitor is null)
        {
            throw new ArgumentNullException(nameof(monitor));
        }

        return new AnonymousObservable<T>(o => monitor.OnChange(o.OnNext)!);
    }
}
