namespace Nodes;

public class ArcNode : Figure
{
    public string Name { get; }
    public Node Center { get; }
    public Node P1 { get; }
    public Node P2 { get; }
    public Node Measure { get; }
    public Node Comment { get; }

    public ArcNode(string name, Node center, Node p1, Node p2, Node measure, Node comment) : base(name)
    {
        Name = name;
        Center = center;
        P1 = p1;
        P2 = p2;
        Measure = measure;
        Comment = comment;
    }
}
