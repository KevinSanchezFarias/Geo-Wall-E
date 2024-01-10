namespace Nodes
{
    public class MultipleVariableDeclarationNode : Node
    {
        public List<ConstDeclarationNode> VariableDeclarations { get; } = new();
        public Node Body { get; }

        public MultipleVariableDeclarationNode(List<ConstDeclarationNode> variableDeclarations, Node body)
        {
            VariableDeclarations = variableDeclarations;
            Body = body;
        }
    }
}