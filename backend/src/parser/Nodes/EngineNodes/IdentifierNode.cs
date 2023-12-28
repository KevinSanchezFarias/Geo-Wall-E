namespace Nodes;

/// <summary>
/// Represents a node that represents an identifier.
/// </summary>
public class IdentifierNode : Node
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public string Identifier { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifierNode"/> class with the specified identifier.
    /// </summary>
    /// <param name="identifier">The identifier value.</param>
    public IdentifierNode(string identifier)
    {
        Identifier = identifier;
    }
}
