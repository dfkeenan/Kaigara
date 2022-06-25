using System;

namespace Kaigara.Avalonia.Controls;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class NumericRangeAttribute : Attribute
{
    public NumericRangeAttribute()
    {

    }

    public NumericRangeAttribute(double minValue, double maxValue, double increment)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        Increment = increment;
    }

    public NumericRangeAttribute(double minValue, double maxValue, double increment, int decimals)
    {
        MinValue = minValue;
        MaxValue = maxValue;
        Increment = increment;
        Decimals = decimals;
    }

    public double? MinValue { get; set; }
    public double? MaxValue { get; set; }
    public double? Increment { get; set; }
    public int? Decimals { get; set; }
}
