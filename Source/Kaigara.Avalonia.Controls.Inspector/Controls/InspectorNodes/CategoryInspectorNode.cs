namespace Kaigara.Avalonia.Controls.InspectorNodes;
public class CategoryInspectorNode : InspectorNode
{
    public CategoryInspectorNode(InspectorContext context, InspectorNodeProvider provider, InspectorNode? parent, string displayName, IEnumerable<MemberInspectorNode> members)
        : base(context, provider, parent, displayName)
    {
        Members = members;
        IsExpanded = true;
    }

    public IEnumerable<MemberInspectorNode> Members { get; }

    public override IEnumerable<InspectorNode> Children => Members;

    public override void Invalidate()
    {

    }
}
