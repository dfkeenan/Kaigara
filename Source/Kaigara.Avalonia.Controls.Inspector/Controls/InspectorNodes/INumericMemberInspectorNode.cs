namespace Kaigara.Avalonia.Controls.InspectorNodes;

public interface INumericMemberInspectorNode
{
    bool HasRange { get; }
    double MaxValue { get; }
    double MinValue { get; }
    double Increment { get; }
    int Decimals { get; }
    string FormatString { get; }
}