using System.Reactive.Subjects;
using System.Diagnostics.CodeAnalysis;

namespace Kaigara.Reactive;

public class ReadOnlyBehavourSubject<T> : IReadOnlyBehavourSubject<T>, IObserver<T>
{
    private readonly BehaviorSubject<T> subject;

    public ReadOnlyBehavourSubject(T value)
        : this(new BehaviorSubject<T>(value))
    {

    }

    public ReadOnlyBehavourSubject(BehaviorSubject<T> subject)
    {
        this.subject = subject ?? throw new ArgumentNullException(nameof(subject));
    }

    public T Value => subject.Value;

    public bool TryGetValue([MaybeNullWhen(false)] out T value)
        => subject.TryGetValue(out value);

    public IDisposable Subscribe(IObserver<T> observer)
        => subject.Subscribe(observer);

    void IObserver<T>.OnCompleted()
        => subject.OnCompleted();

    void IObserver<T>.OnError(Exception error)
        => subject.OnError(error);

    void IObserver<T>.OnNext(T value)
        => subject.OnNext(value);
}
