namespace Nodes;

public class IntersectNode : Node
{
    public string SeqName { get; }
    public Node Figure1 { get; }
    public Node Figure2 { get; }
    public List<PointNode> Coords { get; }
    public IntersectNode(string name, Node figure1, Node figure2, List<PointNode> coords)
    {
        SeqName = name;
        Figure1 = figure1;
        Figure2 = figure2;
        Coords = coords;
    }
}