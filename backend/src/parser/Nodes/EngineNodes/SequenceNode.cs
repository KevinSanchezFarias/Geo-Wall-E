namespace Nodes;

public class SequenceNode : Node
{
    public string Identifier { get; }
    public List<Node> Nodes { get; }

    public SequenceNode(List<Node> nodes, string identifier)
    {
        Nodes = nodes;
        Identifier = identifier;
    }
}
