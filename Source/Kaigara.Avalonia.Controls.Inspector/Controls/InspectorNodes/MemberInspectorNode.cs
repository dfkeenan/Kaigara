using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive;
using System.Reflection;
using Kaigara.Reflection;
using ReactiveUI;

namespace Kaigara.Avalonia.Controls.InspectorNodes;

public class MemberInspectorNode<T> : MemberInspectorNode
{
    public MemberInspectorNode(InspectorContext context, InspectorNodeProvider provider, ObjectInspectorNode parent, MemberInfo memberInfo, object[] index = null)
        : base(context, provider, parent, memberInfo, index)
    {
    }

    public virtual T Value
    {
        get { return (T)GetValue()!; }
        set { this.RaisePropertyChanging(); SetValue(value); this.RaisePropertyChanged(); }
    }

    public override void Invalidate()
    {
        base.Invalidate();
        this.RaisePropertyChanged(nameof(Value));
    }
}

public class MemberInspectorNode : InspectorNode
{
    private readonly object[] index;
    private readonly Func<object, object?> getter;
    private readonly Action<object, object?> setter;
    private readonly InspectorNodeProvider valueNodeProvider = null!;
    private IEnumerable<Type>? construcableTypes;
    private InspectorNode? valueNode;

    public MemberInspectorNode(InspectorContext context, InspectorNodeProvider provider, ObjectInspectorNode parent, MemberInfo memberInfo, object[] index = null)
        : base(context, provider, parent: parent, memberInfo: memberInfo, displayName: $"{memberInfo.GetDisplayName()}{GetIndex(index)}")
    {
        this.index = index;

        if (MemberInfo is PropertyInfo property)
        {
            getter = (obj) => property.GetValue(obj, index);
            setter = (obj, value) => property.SetValue(obj, value, index);
            MemberType = property.PropertyType;
            IsReadOnly = property.IsReadOnly();
        }
        else if (MemberInfo is FieldInfo field)
        {
            getter = field.GetValue;
            setter = field.SetValue;
            MemberType = field.FieldType;
            IsReadOnly = field.IsReadOnly();
        }
        else
        {
            throw new ArgumentException("Member must be Field or Property", nameof(memberInfo));
        }

        valueNodeProvider = Context.GetNodeProvider(MemberType);

        CreateInstance = ReactiveCommand.Create<Type>(CreateNewInstance);

        Invalidate();
    }

    public Type MemberType { get; }

    public Type ValueType => GetValue()?.GetType() ?? MemberType;

    public bool IsReadOnly { get; }

    public bool HasValue => GetValue() is object;

    public bool IsConstructable => ConstructableTypes?.Any() == true;

    public IEnumerable<Type> ConstructableTypes
    {
        get
        {
            construcableTypes ??= Context.GetConstructableTypes(MemberType);
            return construcableTypes;
        }
    }


    private ObjectInspectorNode InstanceNode => (ObjectInspectorNode)this.Parent;

    public InspectorNode? ValueNode 
    { 
        get => valueNode; 
        private set { this.RaiseAndSetIfChanged(ref valueNode, value); }
    }

    public ReactiveCommand<Type, Unit> CreateInstance { get; }

    private void CreateNewInstance(Type type)
    {
        SetValue(Activator.CreateInstance(type));
    }

    public object GetIndex(int i = 0) => index[i];
    public void SetIndex(object value, int i = 0) => index[i] = value;

    private static string? GetIndex(object[] index)
    {
        if (index == null) return null;
        return $"[{string.Join(", ", index)}]";
    }

    public void SetValue(object? value)
    {
        if (IsReadOnly)
        {
            return; //Should we throw exceptions here.
        }

        var oldValue = GetValue();
        setter(InstanceNode.Value, value);
        UpdateValueNode();

        EventArgs args;

        if (InstanceNode is CollectionInspectorNode)
        {
            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldValue, (int)index[0]);
        }
        else
        {
            args = new PropertyChangedEventArgs(MemberInfo.Name);
        }

        Context.OnInspectorNodeChanged(this, new InspectorNodeChangedEventArgs(this, InstanceNode.Value, instanceEventArgs: args));
    }

    public object? GetValue()
    {
        return getter(InstanceNode.Value);
    }

    public override IEnumerable<InspectorNode> GetChildren()
    {
        if (ValueNode == null) yield break;

        yield return ValueNode;
    }

    public override void Invalidate()
    {
        UpdateValueNode();
    }

    private void UpdateValueNode()
    {
        ValueNode = HasValue ? valueNodeProvider?.CreateNode(Context, this, ValueType, index) : null;
        this.RaisePropertyChanged(nameof(HasValue));
    }
}