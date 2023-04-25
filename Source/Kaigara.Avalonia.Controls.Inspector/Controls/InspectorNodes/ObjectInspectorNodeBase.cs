using System.Reflection;

namespace Kaigara.Avalonia.Controls.InspectorNodes;

public class ObjectInspectorNodeBase : InspectorNode
{
    public ObjectInspectorNodeBase(object instance, InspectorContext context, InspectorNodeProvider provider, InspectorNode? parent, MemberInfo memberInfo, string? displayName)
        : base(context, provider, parent, displayName)
    {
        Value = instance ?? throw new ArgumentNullException(nameof(instance));
        MemberInfo = memberInfo ?? throw new ArgumentNullException(nameof(memberInfo));
    }
    public object Value { get; }

    public MemberInfo MemberInfo { get; }

    public Type Type => (Type)MemberInfo;

    public override void Invalidate()
    {

    }
}