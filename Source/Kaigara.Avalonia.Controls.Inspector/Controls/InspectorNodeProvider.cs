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
    //we need content to be object otherwise portable.xaml is crashing
    [Content]
    [TemplateContent]
    public object? Content { get; set; }

    [AssignBinding]
    public BindingBase? ItemsSource { get; set; }

    public IControl Build(object param)
    {
        return param is InspectorNode node ? TemplateContent.Load(node.Provider.Content).Control : null!;
    }

    public InstancedBinding? ItemsSelector(object item)
    {
        if (item is InspectorNode node && node.Provider.ItemsSource is BindingBase itemsSource)
        {
            var obs = itemsSource switch
            {
                Binding reflection => ExpressionObserverBuilder.Build(item, reflection.Path),
                CompiledBindingExtension compiled => new ExpressionObserver(item, compiled.Path.BuildExpression(false)),
                _ => throw new InvalidOperationException("InspectorNodeProvider currently only supports Binding and CompiledBindingExtension!")
            };

            return InstancedBinding.OneWay(obs, BindingPriority.Style);
        }

        return null;
    }

    public bool Match(object data)
    {
        return data is InspectorNode node && node.Provider.Content is { };
    }

    public abstract InspectorNode CreateNode(InspectorContext inspectorContext, InspectorNode parent, MemberInfo memberInfo, object[]? index = null);

    public virtual bool MatchNodeMemberInfo(MemberInfo memberInfo)
    {
        return false;
    }
}
