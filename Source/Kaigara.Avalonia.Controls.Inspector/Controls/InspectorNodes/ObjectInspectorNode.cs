using System.ComponentModel;
using System.Reflection;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Controls.InspectorNodes;
public class ObjectInspectorNode : ObjectInspectorNodeBase
{
    public ObjectInspectorNode(object instance, InspectorContext context, InspectorNodeProvider provider, InspectorNode? parent, MemberInfo memberInfo, string? displayName)
        : base(instance, context, provider, parent, memberInfo, displayName)
    {

        Members = Context.GetMembers(Type).Select(CreateNode).Where(n => n != null).Cast<MemberInspectorNode>().ToList();

        InspectorNode? CreateNode(MemberInfo memberInfo)
        {
            Type memberType = memberInfo.TryGetMemberType()!;

            return Context.GetNodeProvider(memberInfo)?.CreateNode(Context, this, memberInfo, null);
        }

        IsExpanded = true;


        var members = Members.ToLookup(m => m.MemberInfo.GetCustomAttribute<CategoryAttribute>()?.Category ?? "Misc");

        if(members.Count > 1 )
        {
           Categories =  members.Select(m => new CategoryInspectorNode(Context, provider, this, m.Key, m.ToList())).ToList();
        }
    }

    public IEnumerable<MemberInspectorNode> Members { get; }
    public IEnumerable<CategoryInspectorNode>? Categories { get; }

    public override IEnumerable<InspectorNode> Children => (IEnumerable<InspectorNode>?)Categories ?? Members;


}
