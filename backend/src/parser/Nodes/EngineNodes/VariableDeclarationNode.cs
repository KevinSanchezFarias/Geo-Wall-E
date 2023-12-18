namespace Nodes;

public class VariableDeclarationNode : Node
{
    public string Identifier { get; }
    public Node Value { get; }
    public Node Body { get; }

    public VariableDeclarationNode(string identifier, Node value, Node body)
    {
        Identifier = identifier;
        Value = value;
        Body = body;
    }
}
