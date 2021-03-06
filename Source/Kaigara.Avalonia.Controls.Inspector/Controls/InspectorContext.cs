using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;
using Kaigara.Reflection;

namespace Kaigara.Avalonia.Controls;
public class InspectorContext
{
    private readonly Inspector inspector;

    internal InspectorContext(Inspector inspector)
    {
        this.inspector = inspector;
    }

    public InspectorNodeProvider GetNodeProvider(MemberInfo memberInfo)
    {
        //var queue = new PriorityQueue<InspectorNodeProvider, int>();

        var currentTemplateHost = inspector.NodeItemsControl as ILogical;

        while (currentTemplateHost != null)
        {
            if (currentTemplateHost is IDataTemplateHost hostCandidate && hostCandidate.IsDataTemplatesInitialized)
            {
                foreach (var p in hostCandidate.DataTemplates.OfType<InspectorNodeProvider>())
                {
                    if (p.MatchNodeMemberInfo(memberInfo))
                    {
                        return p;//queue.Enqueue(p, p.Priority);
                    }
                }
            }

            currentTemplateHost = currentTemplateHost.LogicalParent;
        }

        IGlobalDataTemplates? global = AvaloniaLocator.Current.GetService<IGlobalDataTemplates>();

        if (global != null && global.IsDataTemplatesInitialized)
        {
            foreach (var p in global.DataTemplates.OfType<InspectorNodeProvider>())
            {
                if (p.MatchNodeMemberInfo(memberInfo))
                {
                    return p;//queue.Enqueue(p, p.Priority);
                }
            }
        }

        return null; // queue.TryDequeue(out var result, out var _) ? result : null;


    }

    public IEnumerable<MemberInfo> GetMembers(Type type)
    {
        var mappedType = inspector.ReflectionContext.MapType(type.GetTypeInfo());

        MemberInfo[] memberInfos = mappedType.GetMembers(BindingFlags.Public | BindingFlags.Instance);
        var mappedMembers = memberInfos
            .Where(m => m is FieldInfo || m is PropertyInfo)
            .ToDictionary(m => m.Name);

        var members = from member in type.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                      where member is FieldInfo field || member is PropertyInfo property
                      let mappedMember = mappedMembers[member.Name]
                      where GetCustomAttribute<InspectorMemberIgnoreAttribute>(mappedMember, true) == null &&
                            ((member is FieldInfo field) ||
                            (member is PropertyInfo property))
                      select member;

        return members;
    }

    public T? GetCustomAttribute<T>(MemberInfo element, bool inherit = false)
        where T : Attribute
    {
        if (element.DeclaringType is null) return null;

        var mappedType = inspector.ReflectionContext.MapType(element.DeclaringType.GetTypeInfo());

        var mappedMember = mappedType.GetMember(element.Name).Single();

        return mappedMember.GetCustomAttributes(inherit).OfType<T>().FirstOrDefault();
    }

    public IEnumerable<Type> GetConstructableTypes(Type memberType)
    {
        if (memberType.IsClass && !memberType.IsAbstract)
        {
            yield return memberType;
        }
        if (memberType.IsInterface || memberType.IsAbstract)
        {
            var searchAssemblies = (inspector.SearchAssemblies ?? Enumerable.Empty<Assembly>()).Prepend(memberType.Assembly);
            var types = searchAssemblies.SelectMany(a => a.GetTypes());
            types = types.Where(t => memberType.IsAssignableFrom(t) && t.IsConstructable());

            foreach (var type in types)
            {
                yield return type;
            }
        }
    }

    internal void OnInspectorNodeChanged(InspectorNode node, InspectorNodeChangedEventArgs inspectorNodeChangedEventArgs)
    {
        inspector.RaiseEvent(inspectorNodeChangedEventArgs);
    }
}
