namespace Nodes;

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
