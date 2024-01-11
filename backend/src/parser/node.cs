

namespace Nodes;

/// <summary>
/// Represents an abstract base class for nodes in the parser.
/// </summary>
public abstract class Node
{
}

/// <summary>
/// Represents an end node in the parser.
/// </summary>
public class EndNode : Node { }
/// <summary>
/// Represents a node that holds a value.
/// </summary>
public class ValueNode : Node
{
    /// <summary>
    /// Gets the value of the node.
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueNode"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value to be stored in the node.</param>
    public ValueNode(object value)
    {
        Value = value;
    }
}
public class Figure : Node
{
    public string name;
    public Figure(string nameX)
    {
        name = nameX;
    }
}
public class MultiAssignmentNode : Node
{
    public List<string> Identifiers { get; }
    public Node Sequence { get; }

    public MultiAssignmentNode(List<string> identifiers, Node sequence)
    {
        Identifiers = identifiers;
        Sequence = sequence;
    }
}
/// <summary>
/// Represents a node that holds an undefined value.
/// </summary>
public class UndefinedNode : Node
{
    /// <summary>
    /// Gets the value of the node.
    /// </summary>
    public static readonly UndefinedNode Value = new();

    /// <summary>
    /// Private constructor to ensure that only one instance of UndefinedNode exists.
    /// </summary>
    private UndefinedNode() { }
}
public class GlobalConstNode : Node
{
    public string Identifier { get; }
    public object Value { get; }

    public GlobalConstNode(string identifier, object value)
    {
        Identifier = identifier;
        Value = value;
    }
}
public class ColorNode : Node
{
    public string Value { get; }

    public ColorNode(string value)
    {
        Value = value;
    }
}
public class RestoreNode : Node
{
    public RestoreNode()
    {
    }
}