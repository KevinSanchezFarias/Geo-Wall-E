namespace Nodes;

public class InfiniteSequenceNode : Node
{
    public string Name { get; }
    public Node StartValueNode { get; }

    public InfiniteSequenceNode(Node startValueNode, string name)
    {
        StartValueNode = startValueNode;
        Name = name;
    }
}
