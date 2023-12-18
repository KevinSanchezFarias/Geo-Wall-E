namespace Nodes;

public class IdentifierNode : Node
{
    public string Identifier { get; }

    public IdentifierNode(string identifier)
    {
        Identifier = identifier;
    }
}
