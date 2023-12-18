namespace Nodes;

public class CircleNode : Node
{
    public string Name { get; }
    public PointNode Center { get; }
    public Node Radius { get; }
    public Node Comment { get; }

    public CircleNode(string name, PointNode cen, Node rad, Node comment)
    {
        Name = name;
        Center = cen;
        Radius = rad;
        Comment = comment;
    }
}
