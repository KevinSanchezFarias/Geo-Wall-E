namespace Nodes;

public class PointNode : Figure
{
    public string Name { get; }
    public Node X { get; }
    public Node Y { get; }

    public PointNode(string name, Node x, Node y) : base(name)
    {
        Name = name;
        X = x;
        Y = y;
    }
}
