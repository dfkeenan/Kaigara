using System;
using System.Reflection;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Controls.InspectorNodes;

public class NumericMemberInspectorNode<T> : MemberInspectorNode<T>, INumericMemberInspectorNode
{
    public NumericMemberInspectorNode(InspectorContext context, InspectorNodeProvider provider, ObjectInspectorNodeBase parent, MemberInfo memberInfo, object[]? index = null)
        : base(context, provider, parent, memberInfo, index)
    {
        TypeCode typeCode = Type.GetTypeCode(Nullable.GetUnderlyingType(MemberType) ?? MemberType);
        if (!typeCode.IsNumericTypeCode())
        {
            throw new ArgumentException("Type not numeric.", nameof(T));
        }

        var range = context.GetCustomAttribute<NumericRangeAttribute>(memberInfo,true);

        HasRange = range is object;
        MinValue = Math.Max(typeCode.GetMinValue(), range?.MinValue ?? double.MinValue);
        MaxValue = Math.Min(typeCode.GetMaxValue(), range?.MaxValue ?? double.MaxValue);
        Decimals = range?.Decimals ?? typeCode.GetDefaultDecimals();
        Increment = range?.Increment ?? typeCode.GetDefaultSmallIncrement();
    }

    public bool HasRange { get; }
    public double MinValue { get; }
    public double MaxValue { get; }
    public double Increment { get; }
    public int Decimals { get; }
    public string FormatString => Decimals > 0 ? $"{{0:0.{new string('0', Decimals)}}}" : "{0:0}";
}
