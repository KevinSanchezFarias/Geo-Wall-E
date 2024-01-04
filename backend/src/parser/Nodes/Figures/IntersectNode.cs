namespace Nodes;

/// <summary>
/// Represents a node that represents the intersection of two figures.
/// </summary>
public class IntersectNode : Node
{
    /// <summary>
    /// Gets the first figure node.
    /// </summary>
    public Node Figure1 { get; }
    /// <summary>
    /// Gets the second figure node.
    /// </summary>
    public Node Figure2 { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IntersectNode"/> class.
    /// </summary>
    /// <param name="figure1">The first figure.</param>
    /// <param name="figure2">The second figure.</param>
    public IntersectNode(Node figure1, Node figure2)
    {
        Figure1 = figure1;
        Figure2 = figure2;
    }
}