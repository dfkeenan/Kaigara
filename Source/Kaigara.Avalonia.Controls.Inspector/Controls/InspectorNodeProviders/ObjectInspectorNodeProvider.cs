using System;
using System.Reflection;
using Kaigara.Avalonia.Controls.InspectorNodes;

namespace Kaigara.Avalonia.Controls.InspectorProviders;

public class ObjectInspectorNodeProvider : InspectorNodeProvider
{
    public override bool MatchNodeMemberInfo(MemberInfo memberInfo)
    {
        return memberInfo is Type type && (type.IsClass || type.IsInterface);
    }

    public override InspectorNode CreateNode(InspectorContext inspectorContext, InspectorNode parent, MemberInfo member, object[]? index = null)
    {
        if (parent is MemberInspectorNode node && node.GetValue() is object obj)
            return new ObjectInspectorNode(obj, inspectorContext, this, parent, (Type)member, "");

        return null;
    }
}
