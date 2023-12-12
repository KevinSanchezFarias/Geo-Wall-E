namespace Nodes;
public abstract class Node { }

public class VariableDeclarationNode(string identifier, Node value, Node body) : Node
{
    public string Identifier { get; } = identifier;
    public Node Value { get; } = value;
    public Node Body { get; } = body;
}
public class MultipleVariableDeclarationNode(List<VariableDeclarationNode> declarations, Node body) : Node
{
    public List<VariableDeclarationNode> Declarations { get; } = declarations;
    public Node Body { get; } = body;
}
public class IdentifierNode(string identifier) : Node
{
    public string Identifier { get; } = identifier;
}
public class ConstDeclarationNode(string identifier, Node value) : Node
{
    public string Identifier { get; } = identifier;
    public Node Value { get; } = value;
}
public class BinaryExpressionNode(Node left, string @operator, Node right) : Node
{
    public Node Left { get; } = left;
    public string Operator { get; } = @operator;
    public Node Right { get; } = right;
}
public class IfExpressionNode(Node condition, Node thenBody, Node elseBody) : Node
{
    public Node Condition { get; } = condition;
    public Node ThenBody { get; } = thenBody;
    public Node ElseBody { get; } = elseBody;
}
public class FunctionPredefinedNode(string name, List<Node> args) : Node
{
    public string Name { get; } = name;
    public List<Node> Args { get; } = args;
}
public class FunctionCallNode(string name, List<Node> args) : Node
{
    public string Name { get; } = name;
    public List<Node> Args { get; } = args;
}
public class FunctionDeclarationNode(string name, List<string> args, Node body) : Node
{
    public string Name { get; } = name;
    public List<string> Args { get; } = args;
    public Node Body { get; } = body;
}
public class PointNode(string name, double X, double Y) : Node
{
    public string Name { get; } = name;
    public double X { get; } = X;
    public double Y { get; } = Y;
}
public class EndNode : Node { }
public class ValueNode(object value) : Node
{
    public object Value { get; } = value;
}
public class CircleNode(string name, PointNode cen, Node rad, Node Comment) : Node
{
    public string Name { get; } = name;
    public PointNode Center { get; } = cen;
    public Node Radius { get; } = rad;
    public Node Comment { get; } = Comment;
}
public class LineNode(string name, PointNode A, PointNode B, Node Comment) : Node
{
    public string Name { get; } = name;
    public PointNode A { get; } = A;
    public PointNode B { get; } = B;
    public Node Comment { get; } = Comment;
}
public class SegmentNode(string name, PointNode A, PointNode B, Node Comment) : Node
{
    public string Name { get; } = name;
    public PointNode A { get; } = A;
    public PointNode B { get; } = B;
    public Node Comment { get; } = Comment;
}
public class RayNode(string name, PointNode p1, PointNode p2, Node comment) : Node
{
    public string Name { get; } = name;
    public PointNode P1 { get; } = p1;
    public PointNode P2 { get; } = p2;
    public Node Comment { get; } = comment;

}
public class ArcNode(string name, PointNode p1, PointNode p2, PointNode p3, Node comment) : Node
{
    public string Name { get; } = name;
    public PointNode P1 { get; } = p1;
    public PointNode P2 { get; } = p2;
    public PointNode P3 { get; } = p3;
    public Node Comment { get; } = comment;
}
public class MeasureNode(PointNode p1, PointNode p2) : Node
{
    public PointNode P1 { get; } = p1;
    public PointNode P2 { get; } = p2;

}
public class DrawNode(Node figures) : Node
{
    public Node Figures { get; } = figures;
}
public class ReturnToDrawNode(Node figures, string color, List<Node> coords) : Node
{
    public Node Figures { get; } = figures;
    public string Color { get; } = color;
    public List<Node> Coords { get; } = coords;
}