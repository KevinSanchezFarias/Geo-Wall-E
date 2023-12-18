namespace Nodes;

public class ReturnToDrawNode : Node
{
    public Node Figures { get; }
    public string Color { get; }
    public List<Node> Coords { get; }

    public ReturnToDrawNode(Node figures, string color, List<Node> coords)
    {
        Figures = figures;
        Color = color;
        Coords = coords;
    }
}
