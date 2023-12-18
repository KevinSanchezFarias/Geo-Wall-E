namespace Nodes;

public class MultipleVariableDeclarationNode : Node
{
    public List<VariableDeclarationNode> Declarations { get; }
    public Node Body { get; }

    public MultipleVariableDeclarationNode(List<VariableDeclarationNode> declarations, Node body)
    {
        Declarations = declarations;
        Body = body;
    }
}
