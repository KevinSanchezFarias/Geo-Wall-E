namespace Nodes
{
    public class MultipleVariableDeclarationNode : Node
    {
        public List<Node> VariableDeclarations { get; } = new();
        public Node Body { get; }

        public MultipleVariableDeclarationNode(List<Node> variableDeclarations, Node body)
        {
            VariableDeclarations = variableDeclarations;
            Body = body;
        }
    }
}