using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Core;
using Avalonia.Markup.Parsers;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;

namespace Kaigara.Avalonia.Controls;
public abstract class InspectorNodeProvider : ITreeDataTemplate
{
    private readonly TreeDataTemplate template = new TreeDataTemplate();

    //we need content to be object otherwise portable.xaml is crashing
    [Content]
    [TemplateContent]
    public object? Content { get; set; }

    [AssignBinding]
    public BindingBase? ItemsSource { get => template.ItemsSource; set => template.ItemsSource = value; }

    public Control? Build(object? param)
    {
        if (param is InspectorNode node)
        {
            var visualTreeForItem = TemplateContent.Load(node.Provider.Content)?.Result;
            if (visualTreeForItem != null)
            {
                visualTreeForItem.DataContext = node;
            }

            return visualTreeForItem;
        }
        else
        {
            return null!;
        }
    }

    public InstancedBinding? ItemsSelector(object? item)
    {
        if (item is InspectorNode node)
        {
          return node.Provider.template.ItemsSelector(item);
        }

        return null;
    }

    public bool Match(object? data)
    {
        return data is InspectorNode node && node.Provider.Content is { };
    }

    public abstract InspectorNode? CreateNode(InspectorContext inspectorContext, InspectorNode parent, MemberInfo memberInfo, object[]? index = null);

    public virtual bool MatchNodeMemberInfo(MemberInfo memberInfo)
    {
        return false;
    }
}
