using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Controls.InspectorNodes;
public class ObjectInspectorNode : InspectorNode
{
    public ObjectInspectorNode(object instance, InspectorContext context, InspectorNodeProvider provider, InspectorNode? parent, MemberInfo memberInfo, string? displayName) 
        : base(context, provider, parent, memberInfo, displayName)
    {
        Value = instance ?? throw new ArgumentNullException(nameof(instance));

        Members = Context.GetMembers(Type).Select(CreateNode).Where(n => n != null).Cast<MemberInspectorNode>().ToList();

        InspectorNode? CreateNode(MemberInfo memberInfo)
        {
            Type memberType = memberInfo.TryGetMemberType()!;

            return Context.GetNodeProvider(memberInfo)?.CreateNode(Context, this, memberInfo, null);
        }
    }

    public Type Type => (Type)MemberInfo;

    public IEnumerable<MemberInspectorNode> Members { get; }

    public object Value { get; }

    public override IEnumerable<InspectorNode> GetChildren()
    {
        return Members;
    }

    public override void Invalidate()
    {
        
    }
}
