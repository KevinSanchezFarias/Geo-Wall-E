namespace Nodes;

public class MeasureNode : Node
{
    public PointNode P1 { get; }
    public PointNode P2 { get; }

    public MeasureNode(PointNode p1, PointNode p2)
    {
        P1 = p1;
        P2 = p2;
    }
}
