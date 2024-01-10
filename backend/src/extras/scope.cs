using Nodes;

namespace WaLI.backend.src.extras
{
    public class Scope
    {
        public Dictionary<string, PointF> poiND { get; private set; }
        public List<DeclaredSequenceNode> Seqs { get; private set; }
        public Dictionary<string, Func<double[], double>> predefinedFunctions { get; private set; }
        public List<ConstDeclarationNode> cDN { get; private set; }
        public List<GlobalConstNode> DeclaredConst { get; private set; }
        public Stack<Brush> Color { get; set; }

        public Scope()
        {
            poiND = new Dictionary<string, PointF>();
            Seqs = new List<DeclaredSequenceNode>();
            predefinedFunctions = new Dictionary<string, Func<double[], double>>();
            cDN = new List<ConstDeclarationNode>();
            DeclaredConst = new List<GlobalConstNode>();
            Color = new Stack<Brush>(new[] { Brushes.White });

            // Initialize predefinedFunctions and DeclaredConst here...
        }
    }
}