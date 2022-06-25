using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Context;

namespace Kaigara.Avalonia.Controls;

public class InspectorReflectionContext : CustomReflectionContext
{
    protected static readonly InspectorMemberIgnoreAttribute CachedIgnoreAttribute = new InspectorMemberIgnoreAttribute();

    public Func<MemberInfo, bool>? IgnoreMember { get; set; }

    protected override IEnumerable<object> GetCustomAttributes(MemberInfo member, IEnumerable<object> declaredAttributes)
    {

        if (IgnoreMember?.Invoke(member) == true)
        {
            yield return CachedIgnoreAttribute;
        }

        foreach (var attribute in declaredAttributes)
        {
            yield return MapAttribute(attribute);
        }
    }

    protected virtual object MapAttribute(object attribute)
    {
        return attribute;
    }
}
