using System.ComponentModel;
using System.Reflection;
using Avalonia.Threading;
using Avalonia.Utilities;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Controls.InspectorNodes;
public class ObjectInspectorNode : ObjectInspectorNodeBase, IWeakEventSubscriber<PropertyChangedEventArgs>
{
    public ObjectInspectorNode(object instance, InspectorContext context, InspectorNodeProvider provider, InspectorNode? parent, MemberInfo memberInfo, string? displayName)
        : base(instance, context, provider, parent, memberInfo, displayName)
    {

        Members = Context.GetMembersForObject(instance).Select(CreateNode).Where(n => n != null).Cast<MemberInspectorNode>().ToList();

        InspectorNode? CreateNode(MemberInfo memberInfo)
        {
            Type memberType = memberInfo.GetMemberType();

            return Context.GetNodeProvider(memberInfo)?.CreateNode(Context, this, memberInfo, null);
        }

        IsExpanded = true;


        var members = Members.ToLookup(m => m.MemberInfo.GetCustomAttribute<CategoryAttribute>()?.Category ?? "Misc");

        if (members.Count > 1)
        {
            Categories = members.Select(m => new CategoryInspectorNode(Context, provider, this, m.Key, m.ToList())).ToList();
        }

        if (instance is INotifyPropertyChanged propertyChanged)
        {
            WeakEvents.ThreadSafePropertyChanged.Subscribe(propertyChanged, this);
        }
    }

    public IEnumerable<MemberInspectorNode> Members { get; }
    public IEnumerable<CategoryInspectorNode>? Categories { get; }

    public override IEnumerable<InspectorNode> Children => (IEnumerable<InspectorNode>?)Categories ?? Members;

    void IWeakEventSubscriber<PropertyChangedEventArgs>.OnEvent(object? sender, WeakEvent ev, PropertyChangedEventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            Members.FirstOrDefault(m => m.MemberInfo.Name == e.PropertyName)?.Invalidate();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (Value is INotifyPropertyChanged propertyChanged)
        {
            WeakEvents.ThreadSafePropertyChanged.Unsubscribe(propertyChanged, this);
        }
    }
}
