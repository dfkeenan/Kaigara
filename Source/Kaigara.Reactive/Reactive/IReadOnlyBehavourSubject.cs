using System.Diagnostics.CodeAnalysis;

namespace Kaigara.Reactive;

public interface IReadOnlyBehavourSubject<T> : IObservable<T>
{
    T Value { get; }

    bool TryGetValue([MaybeNullWhen(false)] out T value);
}
