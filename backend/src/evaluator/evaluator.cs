using Nodes;
using ParserAnalize;
using Lists;

namespace EvaluatorAnalize;


/// <summary>
/// Represents an evaluator for the abstract syntax tree (AST) of a program.
/// </summary>
public class Evaluator
{
    /// <summary>
    /// Represents a node in the abstract syntax tree (AST).
    /// </summary>
    private Node AST { get; set; }
    /// <summary>
    /// Initializes a new instance of the Evaluator class.
    /// </summary>
    /// <param name="ast">The abstract syntax tree representing the program.</param>
    public Evaluator(Node ast) => AST = ast;
    private Dictionary<string, object> variables = new();
    private readonly Stack<Dictionary<string, object>> scopes = new();
    /// <summary>
    /// Evaluates the abstract syntax tree (AST) and returns the result.
    /// </summary>
    /// <returns>The result of the evaluation.</returns>
    public object Evaluate()
    {
        var visited = Visit(AST);
        switch (visited)
        {
            case GlobalConstNode gcn:
                LE.DeclaredConst.Add(gcn);
                return null!;
            case DeclaredSequenceNode dsn:
                LE.Seqs.Add(dsn);
                return null!;
            case Figure figure:
                FigureHandler(figure);
                return null!;
            default:
                return visited;
        }
    }
    /// <summary>
    /// Draws the specified <see cref="DrawNode"/> and returns a list of <see cref="ToDraw"/> objects.
    /// </summary>
    /// <param name="node">The <see cref="DrawNode"/> to be drawn.</param>
    /// <returns>A list of <see cref="ToDraw"/> objects representing the figures to be drawn.</returns>
    public object Draw(DrawNode drawNode)
    {
        var xT = Visit(drawNode.Figures);
        if (drawNode.decl)
        {
            // Handle the case where the circle is declared and drawn in the same statement
            return FBuild((Figure)drawNode.Figures);
        }
        else
        {
            if (drawNode.Figures is GlobalConstNode gcn && gcn.Value is Figure fg)
            {
                return FBuild(fg);
            }
            IdentifierNode x = (IdentifierNode)Visit(new ValueNode(drawNode.Figures));
            return LE.toDraws.Any(p => x.Identifier == p.name) ? LE.toDraws.First(p => x.Identifier == p.name) : null!;
        }

        object FBuild(Figure figure)
        {
            return figure switch
            {
                CircleNode circleNode => CircleBuilder(circleNode),
                LineNode lineNode => LineBuilder(lineNode),
                SegmentNode segmentNode => SegBuilder(segmentNode),
                RayNode rayNode => RayBuilder(rayNode),
                ArcNode arcNode => ArcBuilder(arcNode),
                PointNode pointNode => PointBuilder(pointNode),
                _ => throw new Exception($"Unexpected node type {figure.GetType()}"),
            };
        }
    }
    /// <summary>
    /// Visits a given node and performs the corresponding evaluation logic based on the node type.
    /// </summary>
    /// <param name="node">The node to visit.</param>
    /// <returns>The result of the evaluation.</returns>
    private object Visit(Node node)
    {
        return node switch
        {
            EndNode => null!,
            ValueNode valueNode => valueNode.Value,
            MultiAssignmentNode multiAssignmentNode => MultiAssignmentHandler(multiAssignmentNode),
            ConstDeclarationNode constDeclarationNode => ConstDeclarationNodeHandler(constDeclarationNode),
            GlobalConstNode globalConstNode => GlobalConstNodeHandler(globalConstNode),
            IntersectNode intersectNode => IntersectHandler(intersectNode),
            DrawNode drawNode => Draw(drawNode),
            FunctionCallNode functionCallNode => InvokeDeclaredFunctionsHandler(functionCallNode),
            FunctionPredefinedNode functionPredefinedNode => FunctionPredefinedHandler(functionPredefinedNode),
            MeasureNode measureNode => MeasureNodeHandler(measureNode),
            BinaryExpressionNode binaryExpressionNode => BinaryHandler(binaryExpressionNode),
            IfExpressionNode ifExpressionNode => ConditionalHandler(ifExpressionNode),
            IdentifierNode identifierNode => IdentifierHandler(identifierNode),
            Figure figure => figure,
            VariableDeclarationNode varDecl => VarHandler(varDecl),
            SequenceNode sequenceNode => SequenceHandler(sequenceNode),
            DeclaredSequenceNode declaredSequenceNode => declaredSequenceNode,
            MultipleVariableDeclarationNode multipleVarDecl => MultipleVarHandler(multipleVarDecl),
            _ => throw new Exception($"Unexpected node type {node.GetType()}"),
        };
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
            scopes.Push(new Dictionary<string, object>()); // Enter a new scope

            foreach (var varDecl in multipleVarDecl.Declarations)
            {
                scopes.Peek()[varDecl.Identifier] = Visit(varDecl.Value);
            }

            var result = Visit(multipleVarDecl.Body);

            scopes.Pop(); // Leave the scope

            return result;
        }
        object VarHandler(VariableDeclarationNode varDecl)
        {
            scopes.Push(new Dictionary<string, object>()); // Enter a new scope

            scopes.Peek()[varDecl.Identifier] = Visit(varDecl.Value);

            var result = Visit(varDecl.Body);

            scopes.Pop(); // Leave the scope

            return result;
        }
        object IdentifierHandler(IdentifierNode identifierNode)
        {
            // Check if the variable is in scope
            foreach (var scope in scopes)
            {
                if (scope.TryGetValue(identifierNode.Identifier, out var value))
                {
                    return value;
                }
            }
            if (LE.poiND.Any(p => p.Key == identifierNode.Identifier))
            {
                return LE.poiND.First(p => p.Key == identifierNode.Identifier).Value;
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
            // Evaluate the nodes and cast them to PointF
            var point1 = (PointF)Visit(measureNode.P1);
            var point2 = (PointF)Visit(measureNode.P2);

            // Calculate the distance between the points
            var dx = point2.X - point1.X;
            var dy = point2.Y - point1.Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            return distance;
        }
        object GlobalConstNodeHandler(GlobalConstNode globalConstNode)
        {
            if (globalConstNode.Value is PointNode pointNode)
            {
                return new PointF(Convert.ToSingle(Visit(node: pointNode.X)), Convert.ToSingle(Visit(node: pointNode.Y)));
            }
            var matchingNode = LE.DeclaredConst.FirstOrDefault(node => node.Identifier == globalConstNode.Identifier);
            var x = matchingNode!.Value;
            return x;
        }
        object MultiAssignmentHandler(MultiAssignmentNode multiAssignmentNode)
        {
            // Evaluate the sequence
            var evaluatedSequence = Visit(multiAssignmentNode.Sequence);
            List<object> sequence;

            if (evaluatedSequence is DeclaredSequenceNode declaredSequence)
            {
                sequence = declaredSequence.Nodes;
            }
            else if (evaluatedSequence is List<object> list)
            {
                sequence = list;
            }
            else
            {
                throw new Exception("The right-hand side of a multi-assignment must be a sequence.");
            }

            // Assign values to identifiers
            for (int i = 0; i < multiAssignmentNode.Identifiers.Count; i++)
            {
                var identifier = multiAssignmentNode.Identifiers[i];

                if (i < sequence.Count)
                {
                    // If there is a corresponding value in the sequence, assign it to the identifier
                    var value = sequence[i];
                    LE.DeclaredConst.Add(new GlobalConstNode(identifier: identifier, value: Visit((Node)value)));
                }
            }

            // If there are more values than identifiers, assign the remaining values to a new DeclaredSequenceNode
            if (sequence.Count > multiAssignmentNode.Identifiers.Count)
            {
                var lastIdentifier = multiAssignmentNode.Identifiers.Last();
                var remainingValues = sequence.Skip(multiAssignmentNode.Identifiers.Count - 1).ToList();
                LE.Seqs.Add(new DeclaredSequenceNode(remainingValues, lastIdentifier));
            }

            return null!;
        }
        object ConstDeclarationNodeHandler(ConstDeclarationNode constDeclarationNode)
        {
            if (constDeclarationNode.Value is Figure figure)
            {
                return new GlobalConstNode(constDeclarationNode.Identifier, figure);
            }
            else if (constDeclarationNode.Value is IntersectNode intersectNode)
            {
                var x = Visit(intersectNode);
                if (x is List<PointNode> points)
                {
                    if (points.Count > 1)
                    {
                        return new DeclaredSequenceNode(points.Cast<object>().ToList(), constDeclarationNode.Identifier);
                    }
                    else if (points.Count == 1)
                    {
                        // Create a normal GlobalConstNode with the PointNode
                        return new GlobalConstNode(constDeclarationNode.Identifier, points[0]);
                    }
                    else
                    {
                        throw new Exception("The intersection of two figures must be a point or a sequence of points.");
                    }
                }
            }
            else
            {
                return new GlobalConstNode(constDeclarationNode.Identifier, Visit(constDeclarationNode.Value));
            }
            return null!;
        }
        #endregion
    }

    private object PointNodeHandler(PointNode pointNode)
    {
        if (pointNode.X is null || pointNode.Y is null)
        {
            LE.poiND.Add(pointNode.Name, new PointF(new Random().Next(150, 300), new Random().Next(150, 300)));
            return pointNode;
        }
        else
        {
            var point = PointBuilder(pointNode);
            LE.poiND.Add(pointNode.Name, point.Item2); // Add the PointF to the dictionary
            return pointNode;
        }
    }
    private object CircleNHandler(CircleNode circleNode)
    {
        if (circleNode.Center is null && circleNode.Radius is null)
        {
            LE.ToDraw toDraw = new()
            {
                name = circleNode.name,
                color = LE.Color.First(),
                figure = "CircleNode",
                points = new PointF[] { new(x: new Random().Next(150, 300), y: new Random().Next(150, 300)) },
                rad = new Random().Next(0, 500),
                comment = null!
            };
            LE.toDraws.Add(toDraw);
            return null!;
        }
        else
        {
            LE.toDraws.Add(CircleBuilder(circleNode));
            return null!;
        }
    }
    private object LineNHandler(LineNode lineNode)
    {
        if (lineNode.A is null && lineNode.B is null)
        {
            LE.ToDraw toDraw = new()
            {
                name = lineNode.Name,
                color = LE.Color.First(),
                figure = "LineNode",
                points = new PointF[]
                {
                    new(new Random().Next(150, 300), new Random().Next(150, 300)),
                    new(new Random().Next(150, 300), new Random().Next(150, 300))
                },
                comment = null!
            };
            LE.toDraws.Add(toDraw);
            return null!;
        }
        else
        {
            LE.toDraws.Add(LineBuilder(lineNode));
            return null!;
        }
    }
    private object SegmentNHandler(SegmentNode segmentNode)
    {
        if (segmentNode.A is null && segmentNode.B is null)
        {
            LE.ToDraw toDraw = new()
            {
                name = segmentNode.Name,
                color = LE.Color.First(),
                figure = "SegmentNode",
                points = new PointF[]
                {
                    new(new Random().Next(150, 300), new Random().Next(150, 300)),
                    new(new Random().Next(150, 300), new Random().Next(150, 300))
                },
                comment = null!
            };
            LE.toDraws.Add(toDraw);
            return null!;
        }
        else
        {
            LE.toDraws.Add(SegBuilder(segmentNode));
            return null!;
        }
    }
    private object RayNHandler(RayNode rayNode)
    {
        if (rayNode.P1 is null && rayNode.P2 is null)
        {
            LE.ToDraw toDraw = new()
            {
                name = rayNode.Name,
                color = LE.Color.First(),
                figure = "RayNode",
                points = new PointF[]
                {
                    new(new Random().Next(150, 300), new Random().Next(150, 300)),
                    new(new Random().Next(150, 300), new Random().Next(150, 300))
                },
                comment = null!
            };
            LE.toDraws.Add(toDraw);
            return null!;
        }
        else
        {
            LE.toDraws.Add(RayBuilder(rayNode));
            return null!;
        }
    }
    private object ArcNHandler(ArcNode arcNode)
    {
        if (arcNode.Center is null && arcNode.P1 is null && arcNode.P2 is null && arcNode.Measure is null)
        {
            LE.ToDraw toDraw = new()
            {
                name = arcNode.Name,
                color = LE.Color.First(),
                figure = "ArcNode",
                points = new PointF[]
                {
                    new(new Random().Next(150, 300), new Random().Next(150, 300)),
                    new(new Random().Next(150, 300), new Random().Next(150, 300)),
                    new(new Random().Next(150, 300), new Random().Next(150, 300))
                },
                rad = new Random().Next(0, 500),
                comment = null!
            };
            LE.toDraws.Add(toDraw);
            return null!;
        }
        else
        {
            ArcBuilder(arcNode);
            return null!;
        }
    }
    private object FigureHandler(Figure figure)
    {
        return figure switch
        {
            CircleNode circleNode => CircleNHandler(circleNode),
            LineNode lineNode => LineNHandler(lineNode),
            SegmentNode segmentNode => SegmentNHandler(segmentNode),
            RayNode rayNode => RayNHandler(rayNode),
            ArcNode arcNode => ArcNHandler(arcNode),
            PointNode pointNode => PointNodeHandler(pointNode),
            _ => throw new Exception($"Unexpected node type {figure.GetType()}"),
        };
    }

    private object SequenceHandler(SequenceNode sequenceNode)
    {
        List<object> evaluatedNodes = new();
        foreach (var node in sequenceNode.Nodes)
        {
            object result = Visit(node);
            evaluatedNodes.Add(result);
        }
        DeclaredSequenceNode declaredSequenceNode = new(evaluatedNodes, sequenceNode.Identifier);
        LE.Seqs.Add(declaredSequenceNode);
        return null!;
    }

    private object IntersectHandler(IntersectNode intersectNode)
    {
        var figure1 = Visit(intersectNode.Figure1);
        var figure2 = Visit(intersectNode.Figure2);

        var intersection = CalculateIntersection((Node)figure1, (Node)figure2);

        // Rest of the code...
        return intersection;
    }

    private List<PointNode> CalculateIntersection(Node figure1, Node figure2)
    {
        var intersectionPoints = new List<PointNode>();

        if (figure1 is CircleNode circle1 && figure2 is CircleNode circle2)
        {
            PointF center1 = (PointF)Visit(circle1.Center);
            PointF center2 = (PointF)Visit(circle2.Center);
            double radius1 = (double)Visit(circle1.Radius);
            double radius2 = (double)Visit(circle2.Radius);

            double dx = center2.X - center1.X;
            double dy = center2.Y - center1.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            // Check if there's no solution
            if (distance > radius1 + radius2 || distance < Math.Abs(radius1 - radius2))
            {
                // The circles are separate or one circle is contained within the other
                return intersectionPoints;
            }

            double a = (radius1 * radius1 - radius2 * radius2 + distance * distance) / (2 * distance);
            double h = Math.Sqrt(radius1 * radius1 - a * a);

            double cx2 = center1.X + a * (center2.X - center1.X) / distance;
            double cy2 = center1.Y + a * (center2.Y - center1.Y) / distance;

            // Get the points of intersection
            double intersectionX1 = cx2 + h * (center2.Y - center1.Y) / distance;
            double intersectionY1 = cy2 - h * (center2.X - center1.X) / distance;
            double intersectionX2 = cx2 - h * (center2.Y - center1.Y) / distance;
            double intersectionY2 = cy2 + h * (center2.X - center1.X) / distance;

            intersectionPoints.Add(new PointNode("Intersection1", new ValueNode(intersectionX1), new ValueNode(intersectionY1)));
            intersectionPoints.Add(new PointNode("Intersection2", new ValueNode(intersectionX2), new ValueNode(intersectionY2)));
        }
        else if (figure1 is CircleNode circle && figure2 is LineNode line)
        {
            // Calculate intersection of a circle and a line
        }

        return intersectionPoints;
    }

    #region FigBuilder
    private LE.ToDraw ArcBuilder(ArcNode arcNode)
    {

        LE.ToDraw toDraw = new()
        {
            name = arcNode.Name,
            color = LE.Color.First(),
            figure = "ArcNode",
            points = new PointF[]
            {
                    (PointF)Visit(node: arcNode.Center),
                    (PointF)Visit(node: arcNode.P1),
                    (PointF)Visit(node: arcNode.P2)
            },
            rad = (double)Visit(node: arcNode.Measure),
            comment = null!
        };
        return toDraw;

    }
    private LE.ToDraw SegBuilder(SegmentNode segmentNode)
    {

        LE.ToDraw toDraw = new()
        {
            name = segmentNode.Name,
            color = LE.Color.First(),
            figure = "SegmentNode",
            points = new PointF[]
            {
                (PointF)Visit(node: segmentNode.A),
                (PointF)Visit(node: segmentNode.B)
            },
            comment = null!
        };
        return toDraw;

    }
    private LE.ToDraw RayBuilder(RayNode rayNode)
    {

        LE.ToDraw toDraw = new()
        {
            name = rayNode.Name,
            color = LE.Color.First(),
            figure = "RayNode",
            points = new PointF[]
            {
                    (PointF)Visit(node: rayNode.P1),
                    (PointF)Visit(node: rayNode.P2)
            },
            comment = null!
        };
        return toDraw;

    }
    private LE.ToDraw LineBuilder(LineNode lineNode)
    {
        LE.ToDraw toDraw = new()
        {
            name = lineNode.Name,
            color = LE.Color.First(),
            figure = "LineNode",
            points = new PointF[]
            {
                    (PointF)Visit(node: lineNode.A),
                    (PointF)Visit(node: lineNode.B)
            },
            comment = null!
        };
        return toDraw;
    }
    private LE.ToDraw CircleBuilder(CircleNode circleNode)
    {
        LE.ToDraw toDraw = new()
        {
            name = circleNode.name,
            color = LE.Color.First(),
            figure = "CircleNode",
            points = new PointF[] { (PointF)Visit(node: circleNode.Center) },
            rad = (double)Visit(node: circleNode.Radius),
            comment = null!
        };

        return toDraw;
    }
    private (string, PointF) PointBuilder(PointNode pointNode)
    {
        return (pointNode.Name, new PointF(Convert.ToSingle(Visit(node: pointNode.X)), Convert.ToSingle(Visit(node: pointNode.Y))));
    }
    #endregion
}