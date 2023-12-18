namespace Nodes;

public class RayNode : Node
{
    public string Name { get; }
    public PointNode P1 { get; }
    public PointNode P2 { get; }
    public Node Comment { get; }

    public RayNode(string name, PointNode p1, PointNode p2, Node comment)
    {
        Name = name;
        P1 = p1;
        P2 = p2;
        Comment = comment;
    }
}
