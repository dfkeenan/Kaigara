using Autofac;

namespace Kaigara;

internal class UIComponentGraph
{
    private readonly UIComponentNode root;

    public UIComponentGraph(IComponentContext context)
    {
        root = new UIComponentNode(this, "ROOT");
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
