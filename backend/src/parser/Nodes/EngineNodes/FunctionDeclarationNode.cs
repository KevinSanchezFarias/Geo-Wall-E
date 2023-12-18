namespace Nodes;

public class FunctionDeclarationNode : Node
{
    public string Name { get; }
    public List<string> Args { get; }
    public Node Body { get; }

    public FunctionDeclarationNode(string name, List<string> args, Node body)
    {
        Name = name;
        Args = args;
        Body = body;
    }
}
