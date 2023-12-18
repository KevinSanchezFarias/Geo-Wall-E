namespace Nodes;

public class DrawNode : Node
{
    public Node Figures { get; }

    public DrawNode(Node figures)
    {
        Figures = figures;
    }
}
