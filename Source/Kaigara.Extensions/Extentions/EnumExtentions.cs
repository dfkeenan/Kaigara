using System.Collections;

namespace Kaigara.Extentions;

public static class EnumExtentions
{
    public static IEnumerable<T> GetValues<T>()
        where T : struct, Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static IEnumerable<T> GetFlagsValues<T>()
        where T:    struct, Enum
    {
        return GetFlagsValues(typeof(T)).Cast<T>();
    }

    public static IEnumerable GetFlagsValues(Type enumType)
    {
        return Enum.GetValues(enumType).Cast<int>().Where(e => IsPowerOfTwo(e));
    }

    private static bool IsPowerOfTwo(int n)
    {
        return (n > 0 && ((n & (n - 1)) == 0)) ? true : false;
    }

    public static T Combine<T>(this IEnumerable<T> flags)
        where T : struct, Enum
    {
        return (T)(object)flags.Cast<int>().Aggregate(0, (v, n) => v | n);
    }
}
