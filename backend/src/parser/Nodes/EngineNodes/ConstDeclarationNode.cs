namespace Nodes;

public class ConstDeclarationNode : Node
{
    public string Identifier { get; }
    public Node Value { get; }

    public ConstDeclarationNode(string identifier, Node value)
    {
        Identifier = identifier;
        Value = value;
    }
}
