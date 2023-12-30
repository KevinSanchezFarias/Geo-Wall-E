namespace Nodes;

public class SegmentNode : Figure
{
    public string Name { get; }
    public Node A { get; }
    public Node B { get; }
    public Node Comment { get; }

    public SegmentNode(string name, Node A, Node B, Node Comment) : base(name)
    {
        Name = name;
        this.A = A;
        this.B = B;
        this.Comment = Comment;
    }
}
