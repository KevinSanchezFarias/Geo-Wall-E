namespace Nodes;

public class FunctionCallNode : Node
{
    public string Name { get; }
    public List<Node> Args { get; }

    public FunctionCallNode(string name, List<Node> args)
    {
        Name = name;
        Args = args;
    }
}
