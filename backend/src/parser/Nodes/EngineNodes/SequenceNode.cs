namespace Nodes;

/// <summary>
/// Represents a sequence of nodes.
/// </summary>
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

/// <summary>
/// Represents a declared sequence of nodes.
/// </summary>
public class DeclaredSequenceNode : Node
{
    public string Identifier { get; }
    public List<object> Nodes { get; }

    public DeclaredSequenceNode(List<object> nodes, string identifier)
    {
        Nodes = nodes;
        Identifier = identifier;
    }
}
