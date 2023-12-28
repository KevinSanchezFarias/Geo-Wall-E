
namespace Nodes;

public class InfiniteSequenceNode : Node
{
    public string Name { get; }
    public IEnumerable<double> Sequence { get; }

    public InfiniteSequenceNode(IEnumerable<double> sequence, string name)
    {
        Sequence = sequence;
        Name = name;
    }
}
