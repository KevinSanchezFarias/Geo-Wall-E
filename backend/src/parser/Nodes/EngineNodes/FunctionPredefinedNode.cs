namespace Nodes;

public class FunctionPredefinedNode : Node
{
    public string Name { get; }
    public List<Node> Args { get; }

    public FunctionPredefinedNode(string name, List<Node> args)
    {
        Name = name;
        Args = args;
    }
}
