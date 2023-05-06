using System.Reflection;

namespace Kaigara.Avalonia.Controls.InspectorNodes;

public class ObjectInspectorNodeBase : InspectorNode, IDisposable
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


    private bool isDisposed = false;

    public void Dispose()
    {
        if (isDisposed) return;
        Dispose(true);
        isDisposed = true;
    }

    protected virtual void Dispose(bool disposing)
    {
        
    }

    public override void Invalidate()
    {

    }
}