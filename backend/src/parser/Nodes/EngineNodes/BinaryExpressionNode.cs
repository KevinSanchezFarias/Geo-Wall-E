namespace Nodes;

public class BinaryExpressionNode : Node
{
    public Node Left { get; }
    public string Operator { get; }
    public Node Right { get; }

    public BinaryExpressionNode(Node left, string @operator, Node right)
    {
        Left = left;
        Operator = @operator;
        Right = right;
    }
}
