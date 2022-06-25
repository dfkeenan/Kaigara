using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using ReactiveUI;
using Kaigara.Extentions;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Controls.InspectorNodes;


public class EnumMemberInspectorNode<T> : MemberInspectorNode<T>
    where T : struct, Enum
{

    public EnumMemberInspectorNode(InspectorContext context, InspectorNodeProvider provider, ObjectInspectorNode parent, MemberInfo memberInfo, object[] index = null)
        : base(context, provider, parent, memberInfo, index)
    {      
        EnumValues = EnumExtentions.GetValues<T>().ToList().AsReadOnly();
    }
    
    public ReadOnlyCollection<T> EnumValues { get; }

}


public class FlagsEnumMemberInspectorNode<T> : EnumMemberInspectorNode<T>
    where T : struct, Enum
{
    private bool syncingFlags = false;

    public FlagsEnumMemberInspectorNode(InspectorContext context, InspectorNodeProvider provider, ObjectInspectorNode parent, MemberInfo memberInfo, object[] index = null)
        : base(context, provider, parent, memberInfo, index)
    {
        var value = Value;
        SelectedFlags = new ObservableCollection<T>(EnumValues.Where(v => value.HasFlag(v)));
        SelectedFlags.CollectionChanged += SelectedFlags_CollectionChanged;

        SelectAll = ReactiveCommand.Create(() =>
        {
            SelectedFlags.Clear();
            EnumValues.ToList().ForEach(SelectedFlags.Add);
        }, Observable.Never<bool>().StartWith(!IsReadOnly));

        SelectNone = ReactiveCommand.Create(() =>
        {
            SelectedFlags.Clear();
        }, Observable.Never<bool>().StartWith(!IsReadOnly));
        SelectInverse = ReactiveCommand.Create(() =>
        {
            var inverse = EnumValues.Where(v => !SelectedFlags.Contains(v)).ToList();
            SelectedFlags.Clear();
            inverse.ForEach(SelectedFlags.Add);
        }, Observable.Never<bool>().StartWith(!IsReadOnly));
    }

    private void SelectedFlags_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (!syncingFlags)
        {
            syncingFlags = true;
            Value = SelectedFlags.Combine();
            syncingFlags = false;
        }
    }

    public override T Value
    {
        get => base.Value;
        set
        {
            base.Value = value;
            if (!syncingFlags)
            {
                syncingFlags = true;
                SelectedFlags.Clear();
                EnumValues.Where(v => value.HasFlag(v)).ToList().ForEach(SelectedFlags.Add);
                syncingFlags = false;
            }
        }
    }

    public ObservableCollection<T> SelectedFlags { get; }

    public ReactiveCommand<Unit, Unit> SelectAll { get; }
    public ReactiveCommand<Unit, Unit> SelectNone { get; }
    public ReactiveCommand<Unit, Unit> SelectInverse { get; }
}
