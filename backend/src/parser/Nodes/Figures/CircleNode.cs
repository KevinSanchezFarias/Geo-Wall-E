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

