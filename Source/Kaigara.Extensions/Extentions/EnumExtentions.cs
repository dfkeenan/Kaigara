using System.Runtime.CompilerServices;

namespace Kaigara.Extentions;

public static class EnumExtentions
{
    public static IEnumerable<T> GetValues<T>()
        where T : struct, Enum
    {
        return Enum.GetValues<T>();
    }

    public static IEnumerable<T> GetFlagsValues<T>()
        where T : struct, Enum
    {
        return GetFlagsValues(typeof(T)).Cast<T>();
    }

    public static IEnumerable<object> GetFlagsValues(Type enumType)
    {
        foreach (var item in Enum.GetValues(enumType))
        {
            var value = Convert.ToInt64(item);

            if(IsPowerOfTwo(value))
            {
                yield return Enum.ToObject(enumType, value);
            }
        }
    }

    public static IEnumerable<object> GetFlagsValues(Type enumType, long value)
    {
        foreach (var item in Enum.GetValues(enumType))
        {
            var itemValue = Convert.ToInt64(item);

            if (IsPowerOfTwo(itemValue) && (itemValue & value) != 0)
            {
                yield return Enum.ToObject(enumType, itemValue);
            }
        }
    }

    private static bool IsPowerOfTwo(long n)
    {
        return (n > 0 && ((n & (n - 1)) == 0)) ? true : false;
    }

    public static T Combine<T>(this IEnumerable<T> flags)
        where T : struct, Enum
    {
        return flags.Select(f => f.GetValue()).Aggregate(0L, (v, n) => v | n).GetValue<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long GetValue<T>(this T value)
        where T : struct, Enum
    {
        if (Unsafe.SizeOf<T>() == 1)
        {
            return Unsafe.As<T, byte>(ref value);
        }
        else if (Unsafe.SizeOf<T>() == 2)
        {
            return Unsafe.As<T, short>(ref value);
        }
        else if (Unsafe.SizeOf<T>() == 4)
        {
            return Unsafe.As<T, int>(ref value);
        }
        else if (Unsafe.SizeOf<T>() == 8)
        {
            return Unsafe.As<T, long>(ref value);
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetValue<T>(this long value)
        where T : struct, Enum
    {
        if (Unsafe.SizeOf<T>() == 1)
        {
            byte b = (byte)value;
            return Unsafe.As<byte, T>(ref b);
        }
        else if (Unsafe.SizeOf<T>() == 2)
        {
            short s = (short)value;
            return Unsafe.As<short, T>(ref s);
        }
        else if (Unsafe.SizeOf<T>() == 4)
        {
            int i = (int)value;
            return Unsafe.As<int, T>(ref i);
        }
        else if (Unsafe.SizeOf<T>() == 8)
        {
            return Unsafe.As<long, T>(ref value);
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}
