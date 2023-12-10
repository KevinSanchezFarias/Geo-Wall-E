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
public class CircleNode(PointNode cen, double rad) : Node
{
    public string Name { get; } = "circle";
    public PointNode Center { get; } = cen;
    public double Radius { get; } = rad;
}

public class LineNode(string name, PointNode A, PointNode B, Node Comment) : Node
{
    public string Name { get; } = name;
    public PointNode A { get; } = A;
    public PointNode B { get; } = B;
    public Node Comment { get; } = Comment;
}

public class SegmentNode(List<Node> nodes) : Node
{
    public string Name { get; } = "segment";
    public Node X1 { get; } = nodes[0];
    public Node Y1 { get; } = nodes[1];
    public Node X2 { get; } = nodes[2];
    public Node Y2 { get; } = nodes[3];
}

public class RayNode(List<Node> nodes) : Node
{
    public string Name { get; } = "ray";
    public Node X1 { get; } = nodes[0];
    public Node Y1 { get; } = nodes[1];
    public Node X2 { get; } = nodes[2];
    public Node Y2 { get; } = nodes[3];
}

public class ArcNode(List<Node> nodes) : Node
{
    public string Name { get; } = "arc";
    public Node X1 { get; } = nodes[0];
    public Node Y1 { get; } = nodes[1];
    public Node X2 { get; } = nodes[2];
    public Node Y2 { get; } = nodes[3];
    public Node X3 { get; } = nodes[4];
    public Node Y3 { get; } = nodes[5];
}