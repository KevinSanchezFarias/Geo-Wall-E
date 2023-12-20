namespace Nodes;

/// <summary>
/// Represents an abstract base class for nodes in the parser.
/// </summary>
public abstract class Node { }

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

public class InfiniteSequenceNode : Node
{
    public string Name { get; }
    public IEnumerable<double> Sequence { get; }

    public InfiniteSequenceNode(IEnumerable<double> sequence, string name)
    {
        Sequence = sequence;
        Name = name;
    }

    // Implement the rest of the Node interface here
}
