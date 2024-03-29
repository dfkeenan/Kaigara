﻿using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using Avalonia.Controls.Selection;
using Kaigara.Extentions;
using Kaigara.Reflection;
using ReactiveUI;

namespace Kaigara.Avalonia.Controls.InspectorNodes;


public class EnumMemberInspectorNode<T> : MemberInspectorNode<T>
    where T : struct, Enum
{

    public EnumMemberInspectorNode(InspectorContext context, InspectorNodeProvider provider, ObjectInspectorNodeBase parent, MemberInfo memberInfo, object[]? index = null)
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

    public FlagsEnumMemberInspectorNode(InspectorContext context, InspectorNodeProvider provider, ObjectInspectorNodeBase parent, MemberInfo memberInfo, object[]? index = null)
        : base(context, provider, parent, memberInfo, index)
    {
        var value = Value;
        Selection = new SelectionModel<T>(EnumValues);
        Selection.SingleSelect = false;
        Selection.SelectionChanged += Selection_SelectionChanged;

        for (int i = 0; i < EnumValues.Count; i++)
        {
            if (value.HasFlag(EnumValues[i])) Selection.Select(i);
        }

        SelectAll = ReactiveCommand.Create(() =>
        {
            Selection.SelectAll();
        }, Observable.Never<bool>().StartWith(!IsReadOnly), RxApp.MainThreadScheduler);

        Remove = SelectNone = ReactiveCommand.Create(() =>
        {
            Selection.Clear();
        }, Observable.Never<bool>().StartWith(!IsReadOnly), RxApp.MainThreadScheduler);
        SelectInverse = ReactiveCommand.Create(() =>
        {
            var value = Value;

            for (int i = 0; i < EnumValues.Count; i++)
            {
                if (value.HasFlag(EnumValues[i]))
                    Selection.Deselect(i);
                else
                    Selection.Select(i);
            }
        }, Observable.Never<bool>().StartWith(!IsReadOnly), RxApp.MainThreadScheduler);
    }

    public override void Invalidate()
    {
        if (!syncingFlags)
        {
            var value = Value;

            for (int i = 0; i < EnumValues.Count; i++)
            {
                if (value.HasFlag(EnumValues[i]))
                    Selection.Select(i);
                else
                    Selection.Deselect(i);
            }
        }
        base.Invalidate();
    }


    private void Selection_SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<T> e)
    {
        if (!syncingFlags)
        {
            syncingFlags = true;

            var value = Value.GetValue();

            foreach (var item in e.SelectedItems)
            {
                value |= item.GetValue();
            }

            foreach (var item in e.DeselectedItems)
            {
                value &= ~(item.GetValue());
            }

            Value = value.GetValue<T>();
            syncingFlags = false;
        }
    }

    public SelectionModel<T> Selection { get; }

    public ReactiveCommand<Unit, Unit> SelectAll { get; }
    public ReactiveCommand<Unit, Unit> SelectNone { get; }
    public ReactiveCommand<Unit, Unit> SelectInverse { get; }
}
