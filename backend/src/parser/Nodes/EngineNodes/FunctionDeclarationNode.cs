namespace Nodes;

public class FunctionDeclarationNode : Node
{
    public string Name { get; }
    public List<Node> Args { get; }
    public Node Body { get; }

    public FunctionDeclarationNode(string name, List<Node> args, Node body)
    {
        Name = name;
        Args = args;
        Body = body;
    }
}
