namespace Nodes;

public class DrawNode : Node
{
    public Node Figures { get; }
    public bool decl;

    public DrawNode(Node figures, bool declare)
    {
        decl = declare;
        Figures = figures;
    }
}
