using Avalonia.Interactivity;


namespace Kaigara.Avalonia.Controls;

public class InspectorNodeChangedEventArgs : RoutedEventArgs
{
    public InspectorNodeChangedEventArgs(InspectorNode inspectorNode, object instance, EventArgs instanceEventArgs)
        : base(Inspector.InspectorNodeChangedEvent)
    {
        Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        InstanceEventArgs = instanceEventArgs ?? throw new ArgumentNullException(nameof(instanceEventArgs));
        InspectorNode = inspectorNode;
    }

    public InspectorNode InspectorNode { get; }

    public object Instance { get; }
    public EventArgs InstanceEventArgs { get; }
}
