namespace Kaigara.Reflection;

public static class NumericTypeExtensions
{
    public static bool IsNumericType(this Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (type.IsEnum) return false;

        return Type.GetTypeCode(type)
                   .IsNumericTypeCode();
    }

    public static bool IsNumericTypeCode(this TypeCode typeCode)
        => typeCode > TypeCode.Char && typeCode < TypeCode.Decimal;

    public static double GetMinValue(this TypeCode typeCode)
    {
        switch (typeCode)
        {
            case TypeCode.SByte: return SByte.MinValue;
            case TypeCode.Byte: return Byte.MinValue;
            case TypeCode.Int16: return Int16.MinValue;
            case TypeCode.UInt16: return UInt16.MinValue;
            case TypeCode.Int32: return Int32.MinValue;
            case TypeCode.UInt32: return UInt32.MinValue;
            case TypeCode.Int64: return Int64.MinValue;
            case TypeCode.UInt64: return UInt64.MinValue;
            case TypeCode.Single: return Single.MinValue;
            case TypeCode.Double: return Double.MinValue;
            default:
                throw new ArgumentOutOfRangeException(nameof(typeCode));
        }
    }

    public static double GetMaxValue(this TypeCode typeCode)
    {
        switch (typeCode)
        {
            case TypeCode.SByte: return SByte.MaxValue;
            case TypeCode.Byte: return Byte.MaxValue;
            case TypeCode.Int16: return Int16.MaxValue;
            case TypeCode.UInt16: return UInt16.MaxValue;
            case TypeCode.Int32: return Int32.MaxValue;
            case TypeCode.UInt32: return UInt32.MaxValue;
            case TypeCode.Int64: return Int64.MaxValue;
            case TypeCode.UInt64: return UInt64.MaxValue;
            case TypeCode.Single: return Single.MaxValue;
            case TypeCode.Double: return Double.MaxValue;
            default:
                throw new ArgumentOutOfRangeException(nameof(typeCode));
        }
    }

    public static int GetDefaultDecimals(this TypeCode typeCode)
    {
        switch (typeCode)
        {
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
                return 0;
            case TypeCode.Single:
            case TypeCode.Double:
                return 3;
            default:
                throw new ArgumentOutOfRangeException(nameof(typeCode));
        }
    }

    public static double GetDefaultLargeIncrement(this TypeCode typeCode)
    {
        switch (typeCode)
        {
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
                return 1;
            default:
                throw new ArgumentOutOfRangeException(nameof(typeCode));
        }
    }

    public static double GetDefaultSmallIncrement(this TypeCode typeCode)
    {
        switch (typeCode)
        {
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
                return 1;
            case TypeCode.Single:
            case TypeCode.Double:
                return 0.001;
            default:
                throw new ArgumentOutOfRangeException(nameof(typeCode));
        }
    }
}
