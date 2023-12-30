namespace Nodes;

public class RayNode : Figure
{
    public string Name { get; }
    public Node P1 { get; }
    public Node P2 { get; }
    public Node Comment { get; }

    public RayNode(string name, Node p1, Node p2, Node comment) : base(name)
    {
        Name = name;
        P1 = p1;
        P2 = p2;
        Comment = comment;
    }
}
