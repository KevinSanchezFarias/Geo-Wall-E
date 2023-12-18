using Nodes;
using ParserAnalize;
using Lists;

namespace EvaluatorAnalize;

public struct ToDraw
{
    public string figure;
    public Brush color;
    public Point[] points;
    public double rad;
    public string comment;
}
public class Evaluator
{
    private Node AST { get; set; }

    public Evaluator(Node ast)
    {
        AST = ast;
    }

    private Dictionary<string, object> variables = new();

    public object Evaluate()
    {
        return Visit(AST);
    }
    /// <summary>
    /// Draws the specified <see cref="DrawNode"/> and returns a list of <see cref="ToDraw"/> objects.
    /// </summary>
    /// <param name="node">The <see cref="DrawNode"/> to be drawn.</param>
    /// <returns>A list of <see cref="ToDraw"/> objects representing the figures to be drawn.</returns>
    public object Draw(DrawNode node)
    {
        if (node.Figures is SequenceNode sequenceNode)
        {
            var toDrawList = new List<ToDraw>();
            foreach (var figure in sequenceNode.Nodes)
            {
                switch (figure)
                {
                    case CircleNode cNode:
                        BuildCircleNode(toDrawList, cNode);
                        break;
                    case LineNode lineNode:
                        BuildLineNode(toDrawList, lineNode);
                        break;
                    case SegmentNode segmentNode:
                        BuildSegmentNode(toDrawList, segmentNode);
                        break;
                    case RayNode rayNode:
                        BuildRayNode(toDrawList, rayNode);
                        break;
                    case ArcNode arcNode:
                        BuildArcNode(toDrawList, arcNode);
                        break;
                    case PointNode pointNode:
                        BuildPointNode(toDrawList, pointNode);
                        break;
                    default:
                        throw new Exception($"Unexpected node type {figure.GetType()}");
                }
            }
            return toDrawList;
        }
        else
        {
            var points = node.Figures switch
            {
                PointNode pointNode => new() { new Point((int)pointNode.X, (int)pointNode.Y) },
                CircleNode cNode => new() { new Point((int)cNode.Center.X, (int)cNode.Center.Y) },
                LineNode lineNode => new() { new Point((int)lineNode.A.X, (int)lineNode.A.Y), new Point((int)lineNode.B.X, (int)lineNode.B.Y) },
                SegmentNode segmentNode => new() { new Point((int)segmentNode.A.X, (int)segmentNode.A.Y), new Point((int)segmentNode.B.X, (int)segmentNode.B.Y) },
                RayNode rayNode => new() { new Point((int)rayNode.P1.X, (int)rayNode.P1.Y), new Point((int)rayNode.P2.X, (int)rayNode.P2.Y) },
                ArcNode arcNode => new List<Point> { new((int)arcNode.P1.X, (int)arcNode.P1.Y), new((int)arcNode.P2.X, (int)arcNode.P2.Y), new((int)arcNode.P3.X, (int)arcNode.P3.Y) },
                _ => throw new Exception($"Unexpected node type {node.Figures.GetType()}")
            };
            var comment = node.Figures switch
            {
                CircleNode cNode => cNode.Comment as ValueNode,
                LineNode lineNode => lineNode.Comment as ValueNode,
                SegmentNode segmentNode => segmentNode.Comment as ValueNode,
                RayNode rayNode => rayNode.Comment as ValueNode,
                ArcNode arcNode => arcNode.Comment as ValueNode,
                _ => null
            };
            var toDraw = new ToDraw
            {
                color = LE.Color.First(),
                figure = node.Figures.GetType().Name,
                points = points.ToArray(),
                rad = node.Figures is CircleNode circleNode ? (double)Visit(circleNode.Radius) : 0,
                comment = comment?.Value.ToString() ?? ""
            };
            return toDraw;
        }
        #region BuildFinalNodes
        void BuildCircleNode(List<ToDraw> toDrawList, CircleNode cNode)
        {
            var points = new List<Point> { new((int)cNode.Center.X, (int)cNode.Center.Y) };
            var comment = cNode.Comment as ValueNode;
            var toDraw = new ToDraw
            {
                color = LE.Color.First(),
                figure = cNode.GetType().Name,
                points = points.ToArray(),
                rad = (double)Visit(cNode.Radius),
                comment = comment?.Value.ToString() ?? ""
            };
            toDrawList.Add(toDraw);
        }
        void BuildLineNode(List<ToDraw> toDrawList, LineNode lineNode)
        {
            var points = new List<Point> { new((int)lineNode.A.X, (int)lineNode.A.Y), new((int)lineNode.B.X, (int)lineNode.B.Y) };
            var comment = lineNode.Comment as ValueNode;
            var toDraw = new ToDraw
            {
                color = LE.Color.First(),
                figure = lineNode.GetType().Name,
                points = points.ToArray(),
                rad = 0,
                comment = comment?.Value.ToString() ?? ""
            };
            toDrawList.Add(toDraw);
        }
        void BuildSegmentNode(List<ToDraw> toDrawList, SegmentNode segmentNode)
        {
            var points = new List<Point> { new((int)segmentNode.A.X, (int)segmentNode.A.Y), new((int)segmentNode.B.X, (int)segmentNode.B.Y) };
            var comment = segmentNode.Comment as ValueNode;
            var toDraw = new ToDraw
            {
                color = LE.Color.First(),
                figure = segmentNode.GetType().Name,
                points = points.ToArray(),
                rad = 0,
                comment = comment?.Value.ToString() ?? ""
            };
            toDrawList.Add(toDraw);
        }
        void BuildRayNode(List<ToDraw> toDrawList, RayNode rayNode)
        {
            var points = new List<Point> { new((int)rayNode.P1.X, (int)rayNode.P1.Y), new((int)rayNode.P2.X, (int)rayNode.P2.Y) };
            var comment = rayNode.Comment as ValueNode;
            var toDraw = new ToDraw
            {
                color = LE.Color.First(),
                figure = rayNode.GetType().Name,
                points = points.ToArray(),
                rad = 0,
                comment = comment?.Value.ToString() ?? ""
            };
            toDrawList.Add(toDraw);
        }
        void BuildArcNode(List<ToDraw> toDrawList, ArcNode arcNode)
        {
            var points = new List<Point> { new((int)arcNode.P1.X, (int)arcNode.P1.Y), new((int)arcNode.P2.X, (int)arcNode.P2.Y), new((int)arcNode.P3.X, (int)arcNode.P3.Y) };
            var comment = arcNode.Comment as ValueNode;
            var toDraw = new ToDraw
            {
                color = LE.Color.First(),
                figure = arcNode.GetType().Name,
                points = points.ToArray(),
            };
            toDrawList.Add(toDraw);
        }
        void BuildPointNode(List<ToDraw> toDrawList, PointNode pointNode)
        {
            var points = new List<Point> { new((int)pointNode.X, (int)pointNode.Y) };
            var toDraw = new ToDraw
            {
                color = LE.Color.First(),
                figure = pointNode.GetType().Name,
                points = points.ToArray(),
                rad = 0,
            };
            toDrawList.Add(toDraw);
        }
        #endregion
    }
    private object Visit(Node node)
    {
        switch (node)
        {
            case EndNode:
                return null!;
            case ValueNode valueNode:
                return valueNode.Value;
            case ConstDeclarationNode constDeclarationNode:
                return ConstDeclarationNodeHandler(constDeclarationNode);
            case IntersectNode intersectNode:
                return IntersectHandler(intersectNode);
            case DrawNode drawNode:
                return Draw(drawNode);
            case FunctionCallNode functionCallNode:
                return InvokeDeclaredFunctionsHandler(functionCallNode);
            case FunctionPredefinedNode functionPredefinedNode:
                return FunctionPredefinedHandler(functionPredefinedNode);
            case MeasureNode measureNode:
                return MeasureNodeHandler(measureNode);
            case BinaryExpressionNode binaryExpressionNode:
                return BinaryHandler(binaryExpressionNode);
            case IfExpressionNode ifExpressionNode:
                return ConditionalHandler(ifExpressionNode);
            case IdentifierNode identifierNode:
                return IdentifierHandler(identifierNode);
            case VariableDeclarationNode varDecl:
                return VarHandler(varDecl);
            case MultipleVariableDeclarationNode multipleVarDecl:
                return MultipleVarHandler(multipleVarDecl);
            default:
                throw new Exception($"Unexpected node type {node.GetType()}");
        }
        #region EvaluatorMethods
        object BinaryHandler(BinaryExpressionNode binaryExpressionNode)
        {
            var left = Visit(binaryExpressionNode.Left);
            if (left is ValueNode vn) left = vn.Value;
            var right = Visit(binaryExpressionNode.Right);
            if (right is ValueNode vn2) right = vn2.Value;
            if (binaryExpressionNode.Operator == "+" && left is string leftStr && right is string rightStr)
            {
                return leftStr + rightStr;
            }
            else if (binaryExpressionNode.Operator == "==" && left is string leftStr2 && right is string rightStr2)
            {
                return leftStr2 == rightStr2;
            }
            else
            {
                if (binaryExpressionNode.Operator == "!=" && left is string leftStr3 && right is string rightStr3)
                {
                    return leftStr3 != rightStr3;
                }
                else
                {
                    return left is double leftNum && right is double rightNum
                        ? binaryExpressionNode.Operator switch
                        {
                            "+" => leftNum + rightNum,
                            "-" => leftNum - rightNum,
                            "*" => leftNum * rightNum,
                            "/" => leftNum / rightNum,
                            "%" => leftNum % rightNum,
                            "^" => Math.Pow(leftNum, rightNum),
                            "<" => leftNum < rightNum,
                            ">" => leftNum > rightNum,
                            "<=" => leftNum <= rightNum,
                            ">=" => leftNum >= rightNum,
                            "==" => leftNum == rightNum,
                            "!=" => leftNum != rightNum,
                            _ => throw new Exception($"Unexpected operator {binaryExpressionNode.Operator}")
                        }
                        : throw new Exception($"Invalid operands for operator {binaryExpressionNode.Operator}");
                }
            }
        }
        object ConditionalHandler(IfExpressionNode ifExpressionNode)
        {
            var condition = Visit(ifExpressionNode.Condition);
            if (condition is bool conditionBool)
            {
                return conditionBool ? Visit(ifExpressionNode.ThenBody) : Visit(ifExpressionNode.ElseBody);
            }
            else
            {
                throw new Exception($"Invalid condition type {condition.GetType()}");
            }
        }
        object MultipleVarHandler(MultipleVariableDeclarationNode multipleVarDecl)
        {
            foreach (var varDecl in multipleVarDecl.Declarations)
            {
                variables[varDecl.Identifier] = Visit(varDecl.Value);
            }
            return Visit(multipleVarDecl.Body);
        }
        object VarHandler(VariableDeclarationNode varDecl)
        {
            variables[varDecl.Identifier] = Visit(varDecl.Value);
            return Visit(varDecl.Body);
        }
        object IdentifierHandler(IdentifierNode identifierNode)
        {
            if (variables.TryGetValue(identifierNode.Identifier, out var value))
            {
                return value;
            }
            else if (LE.cDN.Any(c => c.Identifier == identifierNode.Identifier))
            {
                if (LE.cDN.First(c => c.Identifier == identifierNode.Identifier).Value is ValueNode vnc)
                {
                    return vnc.Value;
                }
                else
                {
                    var constNode = LE.cDN.First(c => c.Identifier == identifierNode.Identifier);
                    return constNode.Value is Node ConstNode ? Visit(ConstNode) : constNode.Value;
                }
            }
            else
            {
                throw new Exception($"Undefined variable {identifierNode.Identifier}");
            }
        }
        object FunctionPredefinedHandler(FunctionPredefinedNode functionPredefinedNode)
        {
            if (LE.predefinedFunctions.TryGetValue(functionPredefinedNode.Name, out var function))
            {
                var argValues = functionPredefinedNode.Args.Select(arg => (double)Visit(arg)).ToArray();
                return function(argValues);
            }
            else
            {
                throw new Exception($"Undefined function {functionPredefinedNode.Name}");
            }
        }
        object InvokeDeclaredFunctionsHandler(FunctionCallNode functionCallNode)
        {
            // Find the function declaration
            var functionDeclaration = Parser.FDN.FirstOrDefault(f => f.Name == functionCallNode.Name) ?? throw new Exception($"Undefined function {functionCallNode.Name}");

            // Check the number of arguments
            if (functionDeclaration.Args.Count != functionCallNode.Args.Count)
            {
                throw new Exception($"Incorrect number of arguments for function {functionCallNode.Name}");
            }

            // Bind the arguments to the parameters
            var oldVariables = new Dictionary<string, object>(variables);
            for (int i = 0; i < functionDeclaration.Args.Count; i++)
            {
                var argName = functionDeclaration.Args[i];
                var argValue = Visit(functionCallNode.Args[i]);
                variables[argName] = argValue;
            }

            // Evaluate the function body
            var result = Visit(functionDeclaration.Body);

            // Restore the old variables
            variables = oldVariables;

            return result;
        }
        object MeasureNodeHandler(MeasureNode measureNode)
        {
            if (measureNode.P1 is PointNode pointNodeA && measureNode.P2 is PointNode pointNodeB)
            {
                var x = pointNodeA.X - pointNodeB.X;
                var y = pointNodeA.Y - pointNodeB.Y;
                return Math.Sqrt((x * x) + (y * y));
            }
            throw new Exception($"Invalid measure node {measureNode} passed to measure handler, is it possible to even get here?");
        }
        #endregion
    }
    private object IntersectHandler(IntersectNode intersectNode)
    {
        // Evaluate the intersection of the two figures
        var intersectionPoints = EvaluateIntersection(intersectNode.Figure1, intersectNode.Figure2);
        // Create a PointNode for each intersection point and add it to a list
        var pointNodes = intersectionPoints.Select((point, index) => new PointNode($"Point{index}", point.X, point.Y)).ToList();
        // Cast the list of PointNode objects to a list of Node objects
        var nodes = pointNodes.Cast<Node>().ToList();
        // Create a SequenceNode with the nodes
        var sequenceNode = new SequenceNode(nodes, intersectNode.SeqName);
        // Add the SequenceNode to Seqs
        LE.Seqs.Add(sequenceNode);
        return null!;
    }
    private IEnumerable<PointNode> EvaluateIntersection(Node figure1, Node figure2)
    {
        var intersectionPoints = new List<PointNode>();
        #region AvailableFigures
        if (figure1 is CircleNode circle1 && figure2 is CircleNode circle2)
        {
            double c1r = (double)Visit(circle1.Radius);
            double c2r = (double)Visit(circle2.Radius);
            double dx = circle2.Center.X - circle1.Center.X;
            double dy = circle2.Center.Y - circle1.Center.Y;
            double d = Math.Sqrt(dx * dx + dy * dy);

            // Check if there's no solution
            if (d > c1r + c2r)
            {
                // The circles are separate
                return intersectionPoints;
            }
            if (d < Math.Abs(c1r - c2r))
            {
                // One circle is contained within the other
                return intersectionPoints;
            }

            double a = (c1r * c1r - c2r * c2r + d * d) / (2.0 * d);
            double h = Math.Sqrt(c1r * c1r - a * a);

            double cx2 = circle1.Center.X + a * (dx) / d;
            double cy2 = circle1.Center.Y + a * (dy) / d;

            // Get the points of intersection
            double intersectionX1 = cx2 + h * (dy) / d;
            double intersectionY1 = cy2 - h * (dx) / d;
            double intersectionX2 = cx2 - h * (dy) / d;
            double intersectionY2 = cy2 + h * (dx) / d;

            intersectionPoints.Add(new PointNode("Intersection1", intersectionX1, intersectionY1));
            intersectionPoints.Add(new PointNode("Intersection2", intersectionX2, intersectionY2));
        }
        if (figure1 is CircleNode circle3 && figure2 is LineNode line1)
        {
            double c1r = (double)Visit(circle3.Radius);
            double dx = line1.B.X - line1.A.X;
            double dy = line1.B.Y - line1.A.Y;
            double dr = Math.Sqrt(dx * dx + dy * dy);
            double D = line1.A.X * line1.B.Y - line1.B.X * line1.A.Y;
            double discriminant = c1r * c1r * dr * dr - D * D;

            // Check if there's no solution
            if (discriminant < 0)
            {
                // The circle and line are separate
                return intersectionPoints;
            }

            double x1 = (D * dy + Math.Sign(dy) * dx * Math.Sqrt(discriminant)) / (dr * dr);
            double x2 = (D * dy - Math.Sign(dy) * dx * Math.Sqrt(discriminant)) / (dr * dr);
            double y1 = (-D * dx + Math.Abs(dy) * Math.Sqrt(discriminant)) / (dr * dr);
            double y2 = (-D * dx - Math.Abs(dy) * Math.Sqrt(discriminant)) / (dr * dr);

            intersectionPoints.Add(new PointNode("Intersection1", x1, y1));
            intersectionPoints.Add(new PointNode("Intersection2", x2, y2));
        }
        if (figure1 is CircleNode circle5 && figure2 is RayNode ray1)
        {
            double c1r = (double)Visit(circle5.Radius);
            double dx = ray1.P2.X - ray1.P1.X;
            double dy = ray1.P2.Y - ray1.P1.Y;
            double dr = Math.Sqrt(dx * dx + dy * dy);
            double D = ray1.P1.X * ray1.P2.Y - ray1.P2.X * ray1.P1.Y;
            double discriminant = c1r * c1r * dr * dr - D * D;

            // Check if there's no solution
            if (discriminant < 0)
            {
                // The circle and ray are separate
                return intersectionPoints;
            }

            double x1 = (D * dy + Math.Sign(dy) * dx * Math.Sqrt(discriminant)) / (dr * dr);
            double x2 = (D * dy - Math.Sign(dy) * dx * Math.Sqrt(discriminant)) / (dr * dr);
            double y1 = (-D * dx + Math.Abs(dy) * Math.Sqrt(discriminant)) / (dr * dr);
            double y2 = (-D * dx - Math.Abs(dy) * Math.Sqrt(discriminant)) / (dr * dr);

            intersectionPoints.Add(new PointNode("Intersection1", x1, y1));
            intersectionPoints.Add(new PointNode("Intersection2", x2, y2));
        }
        if (figure1 is CircleNode circle6 && figure2 is ArcNode arc1)
        {
            double c1r = (double)Visit(circle6.Radius);
            double dx = arc1.P2.X - arc1.P1.X;
            double dy = arc1.P2.Y - arc1.P1.Y;
            double dr = Math.Sqrt(dx * dx + dy * dy);
            double D = arc1.P1.X * arc1.P2.Y - arc1.P2.X * arc1.P1.Y;
            double discriminant = c1r * c1r * dr * dr - D * D;

            // Check if there's no solution
            if (discriminant < 0)
            {
                // The circle and arc are separate
                return intersectionPoints;
            }

            double x1 = (D * dy + Math.Sign(dy) * dx * Math.Sqrt(discriminant)) / (dr * dr);
            double x2 = (D * dy - Math.Sign(dy) * dx * Math.Sqrt(discriminant)) / (dr * dr);
            double y1 = (-D * dx + Math.Abs(dy) * Math.Sqrt(discriminant)) / (dr * dr);
            double y2 = (-D * dx - Math.Abs(dy) * Math.Sqrt(discriminant)) / (dr * dr);

            intersectionPoints.Add(new PointNode("Intersection1", x1, y1));
            intersectionPoints.Add(new PointNode("Intersection2", x2, y2));
        }
        if (figure1 is LineNode line3 && figure2 is LineNode line4)
        {
            double a1 = line3.B.Y - line3.A.Y;
            double b1 = line3.A.X - line3.B.X;
            double c1 = a1 * line3.A.X + b1 * line3.A.Y;

            double a2 = line4.B.Y - line4.A.Y;
            double b2 = line4.A.X - line4.B.X;
            double c2 = a2 * line4.A.X + b2 * line4.A.Y;

            double delta = a1 * b2 - a2 * b1;

            // If lines are parallel, the result is a empty list.
            if (delta == 0)
            {
                return new List<PointNode>();
            }

            // Calculate the intersection point
            double x = (b2 * c1 - b1 * c2) / delta;
            double y = (a1 * c2 - a2 * c1) / delta;

            intersectionPoints.Add(new PointNode("Intersection", x, y));
        }
        if (figure1 is LineNode line5 && figure2 is RayNode ray2)
        {
            double a1 = line5.B.Y - line5.A.Y;
            double b1 = line5.A.X - line5.B.X;
            double c1 = a1 * line5.A.X + b1 * line5.A.Y;

            double a2 = ray2.P2.Y - ray2.P1.Y;
            double b2 = ray2.P1.X - ray2.P2.X;
            double c2 = a2 * ray2.P1.X + b2 * ray2.P1.Y;

            double delta = a1 * b2 - a2 * b1;

            // If lines are parallel, the result is a empty list.
            if (delta == 0)
            {
                return new List<PointNode>();
            }

            // Calculate the intersection point
            double x = (b2 * c1 - b1 * c2) / delta;
            double y = (a1 * c2 - a2 * c1) / delta;

            intersectionPoints.Add(new PointNode("Intersection", x, y));
        }
        if (figure1 is LineNode line6 && figure2 is ArcNode arc2)
        {
            double a1 = line6.B.Y - line6.A.Y;
            double b1 = line6.A.X - line6.B.X;
            double c1 = a1 * line6.A.X + b1 * line6.A.Y;

            double a2 = arc2.P2.Y - arc2.P1.Y;
            double b2 = arc2.P1.X - arc2.P2.X;
            double c2 = a2 * arc2.P1.X + b2 * arc2.P1.Y;

            double delta = a1 * b2 - a2 * b1;

            // If lines are parallel, the result is a empty list.
            if (delta == 0)
            {
                return new List<PointNode>();
            }

            // Calculate the intersection point
            double x = (b2 * c1 - b1 * c2) / delta;
            double y = (a1 * c2 - a2 * c1) / delta;

            intersectionPoints.Add(new PointNode("Intersection", x, y));
        }
        if (figure1 is RayNode ray3 && figure2 is RayNode ray4)
        {
            double a1 = ray3.P2.Y - ray3.P1.Y;
            double b1 = ray3.P1.X - ray3.P2.X;
            double c1 = a1 * ray3.P1.X + b1 * ray3.P1.Y;

            double a2 = ray4.P2.Y - ray4.P1.Y;
            double b2 = ray4.P1.X - ray4.P2.X;
            double c2 = a2 * ray4.P1.X + b2 * ray4.P1.Y;

            double delta = a1 * b2 - a2 * b1;

            // If lines are parallel, the result is a empty list.
            if (delta == 0)
            {
                return new List<PointNode>();
            }

            // Calculate the intersection point
            double x = (b2 * c1 - b1 * c2) / delta;
            double y = (a1 * c2 - a2 * c1) / delta;

            intersectionPoints.Add(new PointNode("Intersection", x, y));
        }
        if (figure1 is RayNode ray5 && figure2 is ArcNode arc3)
        {
            double a1 = ray5.P2.Y - ray5.P1.Y;
            double b1 = ray5.P1.X - ray5.P2.X;
            double c1 = a1 * ray5.P1.X + b1 * ray5.P1.Y;

            double a2 = arc3.P2.Y - arc3.P1.Y;
            double b2 = arc3.P1.X - arc3.P2.X;
            double c2 = a2 * arc3.P1.X + b2 * arc3.P1.Y;

            double delta = a1 * b2 - a2 * b1;

            // If lines are parallel, the result is a empty list.
            if (delta == 0)
            {
                return new List<PointNode>();
            }

            // Calculate the intersection point
            double x = (b2 * c1 - b1 * c2) / delta;
            double y = (a1 * c2 - a2 * c1) / delta;

            intersectionPoints.Add(new PointNode("Intersection", x, y));
        }
        if (figure1 is ArcNode arc4 && figure2 is ArcNode arc5)
        {
            double a1 = arc4.P2.Y - arc4.P1.Y;
            double b1 = arc4.P1.X - arc4.P2.X;
            double c1 = a1 * arc4.P1.X + b1 * arc4.P1.Y;

            double a2 = arc5.P2.Y - arc5.P1.Y;
            double b2 = arc5.P1.X - arc5.P2.X;
            double c2 = a2 * arc5.P1.X + b2 * arc5.P1.Y;

            double delta = a1 * b2 - a2 * b1;

            // If lines are parallel, the result is a empty list.
            if (delta == 0)
            {
                return new List<PointNode>();
            }

            // Calculate the intersection point
            double x = (b2 * c1 - b1 * c2) / delta;
            double y = (a1 * c2 - a2 * c1) / delta;

            intersectionPoints.Add(new PointNode("Intersection", x, y));
        }
        if (figure1 is SegmentNode segment1 && figure2 is SegmentNode segment2)
        {
            double a1 = segment1.B.Y - segment1.A.Y;
            double b1 = segment1.A.X - segment1.B.X;
            double c1 = a1 * segment1.A.X + b1 * segment1.A.Y;

            double a2 = segment2.B.Y - segment2.A.Y;
            double b2 = segment2.A.X - segment2.B.X;
            double c2 = a2 * segment2.A.X + b2 * segment2.A.Y;

            double delta = a1 * b2 - a2 * b1;

            // If lines are parallel, the result is a empty list.
            if (delta == 0)
            {
                return new List<PointNode>();
            }

            // Calculate the intersection point
            double x = (b2 * c1 - b1 * c2) / delta;
            double y = (a1 * c2 - a2 * c1) / delta;

            // Check if the intersection point is on both segments
            if (x >= Math.Min(segment1.A.X, segment1.B.X) && x <= Math.Max(segment1.A.X, segment1.B.X) &&
                y >= Math.Min(segment1.A.Y, segment1.B.Y) && y <= Math.Max(segment1.A.Y, segment1.B.Y) &&
                x >= Math.Min(segment2.A.X, segment2.B.X) && x <= Math.Max(segment2.A.X, segment2.B.X) &&
                y >= Math.Min(segment2.A.Y, segment2.B.Y) && y <= Math.Max(segment2.A.Y, segment2.B.Y))
            {
                intersectionPoints.Add(new PointNode("Intersection", x, y));
            }
        }
        #endregion
        return intersectionPoints;
    }
    private object ConstDeclarationNodeHandler(ConstDeclarationNode constDeclarationNode)
    {
        if (constDeclarationNode.Value is ValueNode valueNode)
        {
            variables[constDeclarationNode.Identifier] = valueNode.Value;
        }
        else
        {
            variables[constDeclarationNode.Identifier] = Visit(constDeclarationNode.Value);
        }
        LE.cDN.Add(new ConstDeclarationNode(
            constDeclarationNode.Identifier,
            new ValueNode(Visit(constDeclarationNode.Value))));
        return null!;
    }
}