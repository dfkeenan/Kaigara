namespace Kaigara.Avalonia.Controls;
public static class InspectorNodeHelper
{
    public static InspectorNode? FindNode(this InspectorNode node, Predicate<InspectorNode> predicate)
    {
        var queue = new Queue<InspectorNode>();
        queue.Enqueue(node);
        return FindNode(queue, predicate);
    }

    public static InspectorNode? FindNode(this IEnumerable<InspectorNode> nodes, Predicate<InspectorNode> predicate)
    {
        var queue = new Queue<InspectorNode>(nodes);
        return FindNode(queue, predicate);
    }

    private static InspectorNode? FindNode(Queue<InspectorNode> queue, Predicate<InspectorNode> predicate)
    {
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (predicate(current))
            {
                return current;
            }

            foreach (var child in current.Children)
            {
                queue.Enqueue(child);
            }
        }

        return null;
    }
}
