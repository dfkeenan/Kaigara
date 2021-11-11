using System.Reactive.Linq;

namespace Kaigara.Reactive
{
    public static class ReactiveExtensions
    {
        public static IObservable<bool> Is<T>(this IObservable<object> source)
        {
            return source.Select(e => e is T);
        }

        public static IObservable<bool> Is<T>(this IObservable<object> source, Func<T, IObservable<bool>> condition)
        {
            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return source.SelectMany(e => e is T i ? condition(i) : Observable.Return(false));
        }

    }
}
