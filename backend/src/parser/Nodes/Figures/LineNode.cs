namespace Nodes;

public class LineNode : Node
{
    public string Name { get; }
    public PointNode A { get; }
    public PointNode B { get; }
    public Node Comment { get; }

    public LineNode(string name, PointNode A, PointNode B, Node Comment)
    {
        Name = name;
        this.A = A;
        this.B = B;
        this.Comment = Comment;
    }
}
