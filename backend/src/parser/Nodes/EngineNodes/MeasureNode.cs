namespace Nodes;

public class MeasureNode : Node
{
    public Node P1 { get; }
    public Node P2 { get; }

    public MeasureNode(Node p1, Node p2)
    {
        P1 = p1;
        P2 = p2;
    }
}
