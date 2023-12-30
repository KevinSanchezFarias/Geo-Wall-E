namespace Nodes;

public class LineNode : Figure
{
    public string Name { get; }
    public Node A { get; }
    public Node B { get; }
    public Node Comment { get; }

    public LineNode(string name, Node A, Node B, Node Comment) : base(name)
    {
        Name = name;
        this.A = A;
        this.B = B;
        this.Comment = Comment;
    }
}
