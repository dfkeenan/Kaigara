using System.Reflection;
using Kaigara.Avalonia.Controls.InspectorNodes;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Controls.InspectorProviders;

public class EnumMemberInspectorNodeProvider : InspectorNodeProvider
{
    public bool FlagsEnum { get; set; }
    public override InspectorNode CreateNode(InspectorContext inspectorContext, InspectorNode parent, MemberInfo memberInfo, object[]? index = null)
    {
        Type memberType = memberInfo.GetMemberType()!;
        Type nodeType = FlagsEnum ? typeof(FlagsEnumMemberInspectorNode<>) : typeof(EnumMemberInspectorNode<>);

        return MemberInspectorNodeProvider.CreateNode(inspectorContext, this, nodeType, memberType, parent, memberInfo, index);
    }

    public override bool MatchNodeMemberInfo(MemberInfo memberInfo)
    {
        return memberInfo.TryGetMemberType(out var memberType)
            && memberType.IsEnum && FlagsEnum == memberType.HasAttribute<FlagsAttribute>();
    }
}