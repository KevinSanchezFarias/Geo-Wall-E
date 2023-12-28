
namespace Nodes;

public class ImportNode : Node
{
    public string FilePath { get; }

    public ImportNode(string filePath)
    {
        FilePath = filePath;
    }
}