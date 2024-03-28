using Autofac;

namespace Kaigara;

internal class UIComponentGraph
{
    private readonly UIComponentNode root;

    public UIComponentGraph(IComponentContext context)
    {
        root = new RootUIComponentNode(this, "ROOT");
        ComponentContext = context;
    }

    public IComponentContext ComponentContext { get; }
    public IDisposable Add<TDefinition>(UIComponentLocation location, TDefinition definition)
        where TDefinition : IUIComponentDefinition
        => root.Add(location, definition);

    public IDisposable Add<TDefinition>(TDefinition definition)
        where TDefinition : IUIComponentDefinition
        => root.Add(definition);

    public IUIComponentDefinition? Find(UIComponentLocation location)
        => root.GetNode(location).Definition;

    public IDisposable AddConfiguration<TDefinition>(UIComponentLocation location, Action<TDefinition> options)
        where TDefinition : IUIComponentDefinition
    {
        if (location is null)
        {
            throw new ArgumentNullException(nameof(location));
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        return root.GetNode(location).AddConfiguration(o => options((TDefinition)o));
    }
}

internal class RootUIComponentNode : UIComponentNode
{
    private Dictionary<string, UIComponentNode> nonRootedNodes = new Dictionary<string, UIComponentNode>();


    public RootUIComponentNode(UIComponentGraph graph, string name) 
        : base(graph, name)
    {
    }

    public override IDisposable Add(IUIComponentDefinition definition)
    {
        var result = base.Add(definition);

        foreach (var item in nonRootedNodes.ToList())
        {
            if (TryFind(item.Key, out var node))
            {
                node.Merge(item.Value);

                nonRootedNodes.Remove(item.Key);
            }
        }

        return result;
    }

    public override UIComponentNode GetNode(UIComponentLocation location)
    {
        UIComponentNode? node;

        if(location.IsRelative)
        {
            if (TryFind(location.PathSegments[0], out node))
            {
                foreach (var name in location.PathSegments.Skip(1))
                {
                    node = node.GetOrAddNode(name);
                }

                return node;
            }
            else if (nonRootedNodes.TryGetValue(location.PathSegments[0], out node))
            {
                foreach (var name in location.PathSegments.Skip(1))
                {
                    node = node.GetOrAddNode(name);
                }

                return node;
            }
            else
            {
                node = new UIComponentNode(Graph, location.PathSegments[0]);
                nonRootedNodes.Add(location.PathSegments[0], node);

                foreach (var name in location.PathSegments.Skip(1))
                {
                    node = node.GetOrAddNode(name);
                }

                return node;
            }
        }

        return base.GetNode(location);
    }
}
