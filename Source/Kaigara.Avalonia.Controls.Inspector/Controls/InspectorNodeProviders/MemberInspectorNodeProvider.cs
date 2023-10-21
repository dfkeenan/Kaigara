using System.Reflection;
using Kaigara.Avalonia.Controls.InspectorNodes;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Controls.InspectorProviders;

public class MemberInspectorNodeProvider : InspectorNodeProvider
{
    public Type? MemberType { get; set; }

    public Type? NodeType { get; set; }

    public override InspectorNode CreateNode(InspectorContext inspectorContext, InspectorNode parent, MemberInfo memberInfo, object[]? index = null)
    {
        Type memberType = memberInfo.TryGetMemberType();

        if (NodeType is object)
        {
            return CreateNode(inspectorContext, this, NodeType, parent, memberInfo, index);
        }

        return CreateNode(inspectorContext, this, typeof(MemberInspectorNode<>), memberType, parent, memberInfo, index);
    }

    public override bool MatchNodeMemberInfo(MemberInfo memberInfo)
    {
        return MemberType is object
            && memberInfo.TryGetMemberType()?.EnsureRuntimeType() is Type memberType
            && (MemberType.IsAssignableFrom(memberType) || (Nullable.GetUnderlyingType(memberType)?.IsAssignableFrom(MemberType) == true));
    }

    internal static InspectorNode CreateNode(InspectorContext inspectorContext, InspectorNodeProvider provider, Type nodeType, InspectorNode parent, MemberInfo member, object[]? index)
    {
        return (InspectorNode)(Activator.CreateInstance(nodeType, inspectorContext, provider, parent, member, index) ?? throw new Exception($"Unable to create type {nodeType}"));
    }

    internal static InspectorNode CreateNode(InspectorContext inspectorContext, InspectorNodeProvider provider, Type genericNodeType, Type nodeType, InspectorNode parent, MemberInfo member, object[]? index)
    {
        return CreateNode(inspectorContext, provider, genericNodeType.MakeGenericType(nodeType.EnsureRuntimeType()), parent, member, index);
    }
}
