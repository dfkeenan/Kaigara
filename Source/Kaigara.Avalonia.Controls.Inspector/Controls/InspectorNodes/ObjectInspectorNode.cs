﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
    }

    public IEnumerable<MemberInspectorNode> Members { get; }

    public override IEnumerable<InspectorNode> Children => Members;

    
}
