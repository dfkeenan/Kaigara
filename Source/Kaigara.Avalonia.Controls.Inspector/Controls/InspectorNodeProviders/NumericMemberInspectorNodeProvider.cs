using System.Reflection;
using Kaigara.Avalonia.Controls.InspectorNodes;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Controls.InspectorProviders;

public class NumericMemberInspectorNodeProvider : InspectorNodeProvider
{
    public override InspectorNode CreateNode(InspectorContext inspectorContext, InspectorNode parent, MemberInfo memberInfo, object[]? index = null)
    {
        Type memberType = memberInfo.TryGetMemberType()!;

        return MemberInspectorNodeProvider.CreateNode(inspectorContext, this, typeof(NumericMemberInspectorNode<>), memberType, parent, memberInfo, index);
    }

    public override bool MatchNodeMemberInfo(MemberInfo memberInfo)
    {
        return memberInfo.TryGetMemberType() is Type memberType
            && (memberType.IsNumericType() || Nullable.GetUnderlyingType(memberType)?.IsNumericType() == true);
    }
}