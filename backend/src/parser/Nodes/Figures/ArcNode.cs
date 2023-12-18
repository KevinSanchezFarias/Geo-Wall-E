namespace Nodes;

public class ArcNode : Node
{
    public string Name { get; }
    public PointNode P1 { get; }
    public PointNode P2 { get; }
    public PointNode P3 { get; }
    public Node Comment { get; }

    public ArcNode(string name, PointNode p1, PointNode p2, PointNode p3, Node comment)
    {
        Name = name;
        P1 = p1;
        P2 = p2;
        P3 = p3;
        Comment = comment;
    }
}
