namespace Nodes;

public class IfExpressionNode : Node
{
    public Node Condition { get; }
    public Node ThenBody { get; }
    public Node ElseBody { get; }

    public IfExpressionNode(Node condition, Node thenBody, Node elseBody)
    {
        Condition = condition;
        ThenBody = thenBody;
        ElseBody = elseBody;
    }
}
