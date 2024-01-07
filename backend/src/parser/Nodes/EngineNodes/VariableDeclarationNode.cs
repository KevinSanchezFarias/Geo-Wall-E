namespace Nodes;

public class VariableDeclarationNode : Node
{
    public Node Identifier { get; }
    public Node Value { get; }
    public Node Body { get; }

    public VariableDeclarationNode(Node identifier, Node value, Node body)
    {
        Identifier = identifier;
        Value = value;
        Body = body;
    }
}
