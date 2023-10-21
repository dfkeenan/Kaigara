using ReactiveUI;

namespace Kaigara.Avalonia.Controls;
public abstract class InspectorNode : ReactiveObject
{

    protected InspectorNode(InspectorContext context, InspectorNodeProvider provider, InspectorNode? parent, string? displayName)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        Parent = parent;
        
        DisplayName = displayName;
    }

    public InspectorContext Context { get; }
    public InspectorNodeProvider Provider { get; }
    public InspectorNode? Parent { get; }
    
    public string? DisplayName { get; }

    public virtual bool CanRemove => false;

    private bool isExpanded;
    public bool IsExpanded { get => isExpanded; set => this.RaiseAndSetIfChanged(ref isExpanded, value); }

    public virtual IEnumerable<InspectorNode> Children => Enumerable.Empty<InspectorNode>();

    public abstract void Invalidate();
}
