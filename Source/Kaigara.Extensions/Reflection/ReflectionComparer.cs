using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Kaigara.Reflection;

public static class ReflectionComparer
{
    private static readonly Type GenericEqualityComparer = typeof(EqualityComparer<>);
    private static readonly ConcurrentDictionary<Type, Func<object, object, bool>> equalityComparers = new ConcurrentDictionary<Type, Func<object, object, bool>>();

    public static bool EqualityComparerEquals(object x, object y)
    {
        if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
        {
            return true;
        }

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
        {
            return false;
        }

        var type = (x?.GetType()) ?? (y?.GetType());

        Func<object, object, bool> equalityComparer = GetEqualityComparer(type!);

        return equalityComparer(x!, y!);
    }

    private static Func<object, object, bool> GetEqualityComparer(Type type)
    {
        return equalityComparers.GetOrAdd(type, (t) =>
        {
            var comparer = GenericEqualityComparer.MakeGenericType(t).GetProperty("Default")!.GetValue(null)!;

            var equalsMethodInfo = comparer.GetType().GetMethod("Equals", new Type[] { type, type })!;

            var xP = Expression.Parameter(typeof(object));
            var yP = Expression.Parameter(typeof(object));

            var methodCall = Expression.Call(Expression.Constant(comparer), equalsMethodInfo, Expression.Convert(xP, type), Expression.Convert(yP, type));

            var equalsMethod = Expression.Lambda<Func<object, object, bool>>(methodCall, xP, yP).Compile();

            return equalsMethod;
        });
    }
}
