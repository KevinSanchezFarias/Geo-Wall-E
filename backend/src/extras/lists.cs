global using Lists;
using Nodes;

namespace Lists
{
    public static class LE
    {
        private static Stack<string> color = new(new[] { "black" });
        public static readonly List<PointNode> poiND = new();
        public static readonly List<ArcNode> arcND = new();
        public static readonly List<CircleNode> cirND = new();
        public static readonly List<LineNode> linND = new();
        public static readonly List<SegmentNode> segND = new();
        public static readonly List<RayNode> rayND = new();
        public static readonly List<List<ConstDeclarationNode>> Seqs = new();
        public static readonly Dictionary<string, Func<double[], double>> predefinedFunctions = new()
    {
        { "Sin", args => Math.Sin(args[0]) },
        { "Cos", args => Math.Cos(args[0]) },
        { "Tan", args => Math.Tan(args[0]) },
        { "Sqrt", args => Math.Sqrt(args[0]) },
        { "Pow", args => Math.Pow(args[0], args[1]) },
        { "Abs", args => Math.Abs(args[0]) },
        { "Floor", args => Math.Floor(args[0]) },
        { "Ceiling", args => Math.Ceiling(args[0]) },
        { "Round", args => Math.Round(args[0]) },
        { "Truncate", args => Math.Truncate(args[0]) },
        { "Log", args => args.Length == 1 ? Math.Log(args[0]) : Math.Log(args[0], args[1]) },
        { "Log10", args => Math.Log10(args[0]) },
        { "Exp", args => Math.Exp(args[0]) },
        { "Min", args => args.Min() },
        { "Max", args => args.Max() },
        { "Sum", args => args.Sum() },
        { "Average", args => args.Average() },
        { "Median", args => args.OrderBy(x => x).ElementAt(args.Length / 2) },
        { "Mode", args => args.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key },
        { "Range", args => args.Max() - args.Min() },
        { "Fact", args => Enumerable.Range(1, (int)args[0]).Aggregate(1, (p, item) => p * item) },
        { "Rand", args => new Random().NextDouble() * (args.Length == 1 ? args[0] : args[1] - args[0]) + (args.Length == 1 ? 0 : args[0]) }
    };
        public static readonly List<ConstDeclarationNode> cDN = new()
        {
        new ConstDeclarationNode("PI", new ValueNode(Math.PI)),
        new ConstDeclarationNode("E", new ValueNode(Math.E)),
        new ConstDeclarationNode("G", new ValueNode(6.67430)),
        new ConstDeclarationNode("C", new ValueNode(299792458.0)),
        new ConstDeclarationNode("GAMMA", new ValueNode(0.57721566490153286060651209008240243104215933593992)),
        new ConstDeclarationNode("PHI", new ValueNode(1.61803398874989484820458683436563811772030917980576)),
        new ConstDeclarationNode("K", new ValueNode(1.380649e-23)),
        new ConstDeclarationNode("NA", new ValueNode(6.02214076e23)),
        new ConstDeclarationNode("R", new ValueNode(8.31446261815324)),
        new ConstDeclarationNode("SIGMA", new ValueNode(5.670374419e-8)),
        new ConstDeclarationNode("GOLDENRATIO", new ValueNode(1.61803398874989484820458683436563811772030917980576)),
        new ConstDeclarationNode("AVOGADRO", new ValueNode(6.02214076e23)),
        };
        public static Stack<string> Color { get => color; set => color = value; }
    }
}