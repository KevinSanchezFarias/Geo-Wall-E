namespace Nodes;

public class CircleNode : Figure
{
    public Node Center { get; }
    public Node Radius { get; }
    public Node Comment { get; }

    public CircleNode(string nameX, Node cen, Node rad, Node comment) : base(nameX)
    {
        name = nameX;
        Center = cen;
        Radius = rad;
        Comment = comment;
    }
}

public class EvaluatedCircleNode : CircleNode
{
    public EvaluatedCircleNode(string name, Node center, Node radius, Node comment)
        : base(name, center, radius, null)
    {
    }
}

