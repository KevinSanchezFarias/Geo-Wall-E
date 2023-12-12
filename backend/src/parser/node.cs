namespace Nodes;
public abstract class Node { }

public class VariableDeclarationNode : Node
{
    public string Identifier { get; }
    public Node Value { get; }
    public Node Body { get; }

    public VariableDeclarationNode(string identifier, Node value, Node body)
    {
        Identifier = identifier;
        Value = value;
        Body = body;
    }
}

public class MultipleVariableDeclarationNode : Node
{
    public List<VariableDeclarationNode> Declarations { get; }
    public Node Body { get; }

    public MultipleVariableDeclarationNode(List<VariableDeclarationNode> declarations, Node body)
    {
        Declarations = declarations;
        Body = body;
    }
}

public class IdentifierNode : Node
{
    public string Identifier { get; }

    public IdentifierNode(string identifier)
    {
        Identifier = identifier;
    }
}

public class ConstDeclarationNode : Node
{
    public string Identifier { get; }
    public Node Value { get; }

    public ConstDeclarationNode(string identifier, Node value)
    {
        Identifier = identifier;
        Value = value;
    }
}
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
public class PointNode : Node
{
    public string Name { get; }
    public double X { get; }
    public double Y { get; }

    public PointNode(string name, double x, double y)
    {
        Name = name;
        X = x;
        Y = y;
    }
}
public class EndNode : Node { }
public class ValueNode : Node
{
    public object Value { get; }

    public ValueNode(object value)
    {
        Value = value;
    }
}
public class CircleNode : Node
{
    public string Name { get; }
    public PointNode Center { get; }
    public Node Radius { get; }
    public Node Comment { get; }

    public CircleNode(string name, PointNode cen, Node rad, Node comment)
    {
        Name = name;
        Center = cen;
        Radius = rad;
        Comment = comment;
    }
}
public class LineNode : Node
{
    public string Name { get; }
    public PointNode A { get; }
    public PointNode B { get; }
    public Node Comment { get; }

    public LineNode(string name, PointNode A, PointNode B, Node Comment)
    {
        Name = name;
        this.A = A;
        this.B = B;
        this.Comment = Comment;
    }
}
public class SegmentNode : Node
{
    public string Name { get; }
    public PointNode A { get; }
    public PointNode B { get; }
    public Node Comment { get; }

    public SegmentNode(string name, PointNode A, PointNode B, Node Comment)
    {
        Name = name;
        this.A = A;
        this.B = B;
        this.Comment = Comment;
    }
}
public class RayNode : Node
{
    public string Name { get; }
    public PointNode P1 { get; }
    public PointNode P2 { get; }
    public Node Comment { get; }

    public RayNode(string name, PointNode p1, PointNode p2, Node comment)
    {
        Name = name;
        P1 = p1;
        P2 = p2;
        Comment = comment;
    }
}

public class ArcNode : Node
{
    public string Name { get; }
    public PointNode P1 { get; }
    public PointNode P2 { get; }
    public PointNode P3 { get; }
    public Node Comment { get; }

    public ArcNode(string name, PointNode p1, PointNode p2, PointNode p3, Node comment)
    {
        Name = name;
        P1 = p1;
        P2 = p2;
        P3 = p3;
        Comment = comment;
    }
}
public class MeasureNode : Node
{
    public PointNode P1 { get; }
    public PointNode P2 { get; }

    public MeasureNode(PointNode p1, PointNode p2)
    {
        P1 = p1;
        P2 = p2;
    }
}
public class DrawNode : Node
{
    public Node Figures { get; }

    public DrawNode(Node figures)
    {
        Figures = figures;
    }
}
public class ReturnToDrawNode : Node
{
    public Node Figures { get; }
    public string Color { get; }
    public List<Node> Coords { get; }

    public ReturnToDrawNode(Node figures, string color, List<Node> coords)
    {
        Figures = figures;
        Color = color;
        Coords = coords;
    }
}