using System.Reflection;
using Kaigara.Avalonia.Controls.InspectorNodes;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Controls.InspectorProviders;

public class CollectionInspectorNodeProvider : InspectorNodeProvider
{
    public override bool MatchNodeMemberInfo(MemberInfo memberInfo)
    {
        return memberInfo is Type type
           && type.IsInstanceOfGenericInterface(typeof(IList<>));
    }

    public override InspectorNode CreateNode(InspectorContext inspectorContext, InspectorNode parent, MemberInfo member, object[]? index = null)
    {
        if (parent is MemberInspectorNode node && node.GetValue() is object obj)
            return new CollectionInspectorNode(obj, inspectorContext, this, parent, (Type)member, "");

        return null;
    }
}