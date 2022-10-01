using System.Reflection;
using ReactiveUI;

namespace Kaigara.Avalonia.Controls.InspectorNodes;

public class ValueMemberInspectorNode<T> : MemberInspectorNode<T>
    where T : struct
{

    public ValueMemberInspectorNode(InspectorContext context, InspectorNodeProvider provider, ObjectInspectorNodeBase parent, MemberInfo memberInfo, object[] index = null)
        : base(context, provider, parent, memberInfo, index)
    {
    }

    public override T Value 
    { 
        get => (T)(GetValue() ?? new T()); 
        set => base.Value = value; 
    }

    public override void Invalidate()
    {
        base.Invalidate();
        ((IReactiveObject)this).RaisePropertyChanged((string?)null);
    }
}
