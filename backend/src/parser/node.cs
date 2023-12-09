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
public class PointNode(string name) : Node
{
    public string Name { get; } = name;
}
public class EndNode : Node { }

public class ValueNode(object value) : Node
{
    public object Value { get; } = value;
}