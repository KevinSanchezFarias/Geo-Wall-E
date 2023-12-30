namespace Nodes;

public class PointNode : Node
{
    public string Name { get; }
    public Node X { get; }
    public Node Y { get; }

    public PointNode(string name, Node x, Node y)
    {
        Name = name;
        X = x;
        Y = y;
    }
}
