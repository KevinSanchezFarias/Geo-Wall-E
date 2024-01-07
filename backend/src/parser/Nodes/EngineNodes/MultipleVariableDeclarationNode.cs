namespace Nodes;

public class MultipleVariableDeclarationNode : Node
{
    public Dictionary<string, Node> Scope { get; } = new Dictionary<string, Node>();
    public Node Body { get; }

    public MultipleVariableDeclarationNode(Node body)
    {
        Body = body;
    }
}
