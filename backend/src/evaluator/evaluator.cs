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
    private List<Node> AST { get; set; }
    /// <summary>
    /// Initializes a new instance of the Evaluator class.
    /// </summary>
    /// <param name="ast">The abstract syntax tree representing the program.</param>
    public Evaluator(List<Node> ast) => AST = ast;
    private Dictionary<string, object> variables = new();
    private readonly Stack<Dictionary<string, object>> scopes = new();
    public ListExtrasScoper scope = new();
    /// <summary>
    /// Evaluates the abstract syntax tree (AST) and returns the result.
    /// </summary>
    /// <returns>The result of the evaluation.</returns>
    public IEnumerable<object> Evaluate(ListExtrasScoper Scope)
    {
        foreach (var node in AST)
        {
            var visited = Visit(node, Scope);
            scope = Scope;
            switch (visited)
            {
                case GlobalConstNode gcn:
                    scope.DeclaredConst.Add(gcn);
                    break;
                case DeclaredSequenceNode dsn:
                    scope.Seqs.Add(dsn);
                    break;
                case Figure figure:
                    scope.toDraws.Add((ListExtrasScoper.ToDraw)FigureHandler(figure));
                    break;
                case FunctionDeclarationNode functionDeclarationNode:
                    scope.Funcs.Add(functionDeclarationNode);
                    break;
                default:
                    yield return visited;
                    break;
            }
        }
    }
    /// <summary>
    /// Draws the specified <see cref="DrawNode"/> and returns a list of <see cref="ToDraw"/> objects.
    /// </summary>
    /// <param name="node">The <see cref="DrawNode"/> to be drawn.</param>
    /// <returns>A list of <see cref="ToDraw"/> objects representing the figures to be drawn.</returns>
    public object Draw(DrawNode drawNode)
    {
        var xT = Visit(drawNode.Figures, scope);
        if (drawNode.decl)
        {
            // Handle the case where the circle is declared and drawn in the same statement
            return FBuild((Figure)drawNode.Figures);
        }
        else
        {
            if (xT is GlobalConstNode gcn && gcn.Value is Figure fg)
            {
                return FBuild(fg);
            }
            else
            {
                var id = Visit((Node)xT, scope);
                return FBuild((Figure)id);
            }
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
    private object Visit(Node node, ListExtrasScoper Scope)
    {
        scope = Scope;
        return node switch
        {
            EndNode => null!,
            ValueNode valueNode => valueNode.Value,
            MultiAssignmentNode multiAssignmentNode => MultiAssignmentHandler(multiAssignmentNode),
            ConstDeclarationNode constDeclarationNode => ConstDeclarationNodeHandler(constDeclarationNode),
            GlobalConstNode globalConstNode => GlobalConstNodeHandler(globalConstNode),
            IntersectNode intersectNode => IntersectHandler(intersectNode),
            DrawNode drawNode => Draw(drawNode),
            FunctionDeclarationNode functionDeclarationNode => FunctionDeclarehandler(functionDeclarationNode),
            FunctionCallNode functionCallNode => InvokeDeclaredFunctionsHandler(functionCallNode),
            FunctionPredefinedNode functionPredefinedNode => FunctionPredefinedHandler(functionPredefinedNode),
            MeasureNode measureNode => MeasureNodeHandler(measureNode),
            BinaryExpressionNode binaryExpressionNode => BinaryHandler(binaryExpressionNode),
            IfExpressionNode ifExpressionNode => ConditionalHandler(ifExpressionNode),
            IdentifierNode identifierNode => IdentifierHandler(identifierNode),
            Figure figure => figure,
            VariableDeclarationNode varDecl => LetHandler(varDecl),
            SequenceNode sequenceNode => SequenceHandler(sequenceNode),
            DeclaredSequenceNode declaredSequenceNode => declaredSequenceNode,
            MultipleVariableDeclarationNode multipleVarDecl => MultiLet(multipleVarDecl),
            null => null!,
            _ => throw new Exception($"Unexpected node type {node.GetType()}"),
        };
        #region EvaluatorMethods
        object BinaryHandler(BinaryExpressionNode binaryExpressionNode)
        {
            var left = Visit(binaryExpressionNode.Left, scope);
            if (left is ValueNode vn) left = vn.Value;
            var right = Visit(binaryExpressionNode.Right, scope);
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
            var condition = Visit(ifExpressionNode.Condition, scope);
            if (condition is bool conditionBool)
            {
                return conditionBool ? Visit(ifExpressionNode.ThenBody, scope) : Visit(ifExpressionNode.ElseBody, scope);
            }
            else
            {
                throw new Exception($"Invalid condition type {condition.GetType()}");
            }
        }
        object LetHandler(VariableDeclarationNode varDecl)
        {
            scopes.Push(new Dictionary<string, object>()); // Enter a new scope

            //scopes.Peek()[varDecl.Identifier] = Visit(varDecl.Value);

            var result = Visit(varDecl.Body, scope);

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
            if (scope.poiND.Any(p => p.Key == identifierNode.Identifier))
            {
                return scope.poiND.First(p => p.Key == identifierNode.Identifier).Value;
            }
            else if (scope.DeclaredConst.Any(p => p.Identifier == identifierNode.Identifier))
            {
                return scope.DeclaredConst.First(p => p.Identifier == identifierNode.Identifier).Value;
            }
            else if (scope.Seqs.Any(p => p.Identifier == identifierNode.Identifier))
            {
                return scope.Seqs.First(p => p.Identifier == identifierNode.Identifier).Nodes;
            }
            else
            {
                throw new Exception($"Undefined variable {identifierNode.Identifier}");
            }
        }
        object MeasureNodeHandler(MeasureNode measureNode)
        {
            // Evaluate the nodes and cast them to PointF
            var point1 = (PointF)Visit(measureNode.P1, scope);
            var point2 = (PointF)Visit(measureNode.P2, scope);

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
                return new PointF(Convert.ToSingle(Visit(node: pointNode.X, scope)), Convert.ToSingle(Visit(node: pointNode.Y, scope)));
            }
            var matchingNode = scope.DeclaredConst.FirstOrDefault(node => node.Identifier == globalConstNode.Identifier);
            var x = matchingNode!.Value;
            return x;
        }
        object MultiAssignmentHandler(MultiAssignmentNode multiAssignmentNode)
        {
            // Evaluate the sequence
            var evaluatedSequence = Visit(multiAssignmentNode.Sequence, scope);
            List<object> sequence;

            if (evaluatedSequence is DeclaredSequenceNode declaredSequence)
            {
                sequence = declaredSequence.Nodes;
            }
            else if (evaluatedSequence is List<object> list)
            {
                sequence = list;
            }
            else if (evaluatedSequence is List<PointNode> pnds)
            {
                sequence = pnds.Cast<object>().ToList();
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
                    if (value is PointNode pointNode)
                    {
                        scope.poiND.Add(identifier, new PointF(Convert.ToSingle(Visit(node: pointNode.X, scope)), Convert.ToSingle(Visit(node: pointNode.Y, scope))));
                    }
                    else
                    {
                        scope.DeclaredConst.Add(new GlobalConstNode(identifier: identifier, value: Visit((Node)value, scope)));
                    }
                }
            }

            // If there are more values than identifiers, assign the remaining values to a new DeclaredSequenceNode
            if (sequence.Count > multiAssignmentNode.Identifiers.Count)
            {
                var lastIdentifier = multiAssignmentNode.Identifiers.Last();
                var remainingValues = sequence.Skip(multiAssignmentNode.Identifiers.Count - 1).ToList();
                scope.Seqs.Add(new DeclaredSequenceNode(remainingValues, lastIdentifier));
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
                var x = Visit(intersectNode, scope);
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
                return new GlobalConstNode(constDeclarationNode.Identifier, Visit(constDeclarationNode.Value, scope));
            }
            return null!;
        }
        #endregion
    }
    private object FunctionPredefinedHandler(FunctionPredefinedNode functionPredefinedNode)
    {
        if (scope.predefinedFunctions.TryGetValue(functionPredefinedNode.Name, out var function))
        {
            var argValues = functionPredefinedNode.Args.Select(arg => (double)Visit(arg, scope)).ToArray();
            return function(argValues);
        }
        else
        {
            throw new Exception($"Undefined function {functionPredefinedNode.Name}");
        }
    }
    private object InvokeDeclaredFunctionsHandler(FunctionCallNode functionCallNode)
    {
        // Find the function declaration
        var functionDeclaration = scope.Funcs.FirstOrDefault(f => f.Name == functionCallNode.Name) ?? throw new Exception($"Undefined function {functionCallNode.Name}");
        ListExtrasScoper functionScope = scope;
        // Check the number of arguments
        if (functionDeclaration.Args.Count != functionCallNode.Args.Count)
        {
            throw new Exception($"Incorrect number of arguments for function {functionCallNode.Name}");
        }

        // Bind the arguments to the parameters
        for (int i = 0; i < functionDeclaration.Args.Count; i++)
        {
            IdentifierNode argName = (IdentifierNode)functionDeclaration.Args[i];
            var val = Visit(functionCallNode.Args[i], functionScope);
            if (val is PointNode pointNode)
            {
                functionScope.poiND.Add(argName.Identifier, new PointF(Convert.ToSingle(Visit(node: pointNode.X, functionScope)), Convert.ToSingle(Visit(node: pointNode.Y, functionScope))));
            }
            else if (val is PointF pointF)
            {
                functionScope.poiND.Add(argName.Identifier, pointF);
            }
            else
            {
                functionScope.DeclaredConst.Add(new GlobalConstNode(argName.Identifier, val));
            }
        }
        // Evaluate the function body
        var result = Visit(functionDeclaration.Body, functionScope);

        return result;
    }
    private object FunctionDeclarehandler(FunctionDeclarationNode functionDeclarationNode)
    {
        return functionDeclarationNode;
    }


    private object MultiLet(MultipleVariableDeclarationNode multipleVarDecl)
    {
        // Create a new scope for the let expression
        ListExtrasScoper letScope = scope;

        // Evaluate the constant declarations and add them to the new scope
        foreach (var constDecl in multipleVarDecl.VariableDeclarations)
        {
            var value = Visit(constDecl, letScope);

            // Check if the returned value is a DeclaredSequenceNode
            if (value is DeclaredSequenceNode sequenceNode)
            {
                letScope.Seqs.Add(sequenceNode);
            }
            else if (value is GlobalConstNode gcn)
            {
                letScope.DeclaredConst.Add(gcn);
            }
            // Otherwise, it's a ConstDeclarationNode
            else if (value is ConstDeclarationNode constDeclarationNode)
            {
                letScope.DeclaredConst.Add(new GlobalConstNode(constDeclarationNode.Identifier, new ValueNode(value)));
            }
        }

        // Evaluate the body of the let expression using the new scope
        var result = Visit(multipleVarDecl.Body, letScope);

        return result;
    }

    #region FigureHandlers
    private object PointNodeHandler(PointNode pointNode)
    {
        if (pointNode.X is null || pointNode.Y is null)
        {
            ListExtrasScoper.ToDraw toDraw = new()
            {
                name = pointNode.Name,
                color = scope.Color.First(),
                figure = "PointNode",
                points = new PointF[] { new(new Random().Next(150, 300), new Random().Next(150, 300)) },
                comment = null!
            };
            scope.poiND.Add(pointNode.Name, toDraw.points[0]);
            return toDraw;
        }
        else
        {
            return PointBuilder(pointNode);
        }
    }
    private object CircleNHandler(CircleNode circleNode)
    {
        if (circleNode.Center is null && circleNode.Radius is null)
        {
            ListExtrasScoper.ToDraw toDraw = new()
            {
                name = circleNode.name,
                color = scope.Color.First(),
                figure = "CircleNode",
                points = new PointF[] { new(x: new Random().Next(150, 300), y: new Random().Next(150, 300)) },
                rad = new Random().Next(0, 500),
                comment = null!
            };
            //LE.toDraws.Add(toDraw);
            return toDraw;
        }
        else
        {
            //LE.toDraws.Add(CircleBuilder(circleNode));
            return CircleBuilder(circleNode);
        }
    }
    private object LineNHandler(LineNode lineNode)
    {
        if (lineNode.A is null && lineNode.B is null)
        {
            ListExtrasScoper.ToDraw toDraw = new()
            {
                name = lineNode.Name,
                color = scope.Color.First(),
                figure = "LineNode",
                points = new PointF[]
                {
                        new(new Random().Next(150, 300), new Random().Next(150, 300)),
                        new(new Random().Next(150, 300), new Random().Next(150, 300))
                },
                comment = null!
            };
            scope.toDraws.Add(toDraw);
            return null!;
        }
        else
        {
            scope.toDraws.Add(LineBuilder(lineNode));
            return null!;
        }
    }
    private object SegmentNHandler(SegmentNode segmentNode)
    {
        if (segmentNode.A is null && segmentNode.B is null)
        {
            ListExtrasScoper.ToDraw toDraw = new()
            {
                name = segmentNode.Name,
                color = scope.Color.First(),
                figure = "SegmentNode",
                points = new PointF[]
                {
                        new(new Random().Next(150, 300), new Random().Next(150, 300)),
                        new(new Random().Next(150, 300), new Random().Next(150, 300))
                },
                comment = null!
            };
            scope.toDraws.Add(toDraw);
            return null!;
        }
        else
        {
            scope.toDraws.Add(SegBuilder(segmentNode));
            return null!;
        }
    }
    private object RayNHandler(RayNode rayNode)
    {
        if (rayNode.P1 is null && rayNode.P2 is null)
        {
            ListExtrasScoper.ToDraw toDraw = new()
            {
                name = rayNode.Name,
                color = scope.Color.First(),
                figure = "RayNode",
                points = new PointF[]
                {
                        new(new Random().Next(150, 300), new Random().Next(150, 300)),
                        new(new Random().Next(150, 300), new Random().Next(150, 300))
                },
                comment = null!
            };
            scope.toDraws.Add(toDraw);
            return null!;
        }
        else
        {
            scope.toDraws.Add(RayBuilder(rayNode));
            return null!;
        }
    }
    private object ArcNHandler(ArcNode arcNode)
    {
        if (arcNode.Center is null && arcNode.P1 is null && arcNode.P2 is null && arcNode.Measure is null)
        {
            ListExtrasScoper.ToDraw toDraw = new()
            {
                name = arcNode.Name,
                color = scope.Color.First(),
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
            scope.toDraws.Add(toDraw);
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
    #endregion
    private object SequenceHandler(SequenceNode sequenceNode)
    {
        List<object> evaluatedNodes = new();
        foreach (var node in sequenceNode.Nodes)
        {
            object result = Visit(node, scope);
            evaluatedNodes.Add(result);
        }
        DeclaredSequenceNode declaredSequenceNode = new(evaluatedNodes, sequenceNode.Identifier);
        scope.Seqs.Add(declaredSequenceNode);
        return null!;
    }
    private List<PointNode> IntersectHandler(IntersectNode intersectNode)
    {
        var figure1 = Visit(intersectNode.Figure1, scope);
        var figure2 = Visit(intersectNode.Figure2, scope);

        List<PointNode> intersection = CalculateIntersection((Node)figure1, (Node)figure2);

        // Rest of the code...
        return intersection;
    }

    private List<PointNode> CalculateIntersection(Node figure1, Node figure2)
    {
        var intersectionPoints = new List<PointNode>();

        if (figure1 is CircleNode circle1 && figure2 is CircleNode circle2)
        {
            PointF center1 = (PointF)Visit(circle1.Center, scope);
            PointF center2 = (PointF)Visit(circle2.Center, scope);
            double radius1 = (double)Visit(circle1.Radius, scope);
            double radius2 = (double)Visit(circle2.Radius, scope);

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
    private ListExtrasScoper.ToDraw ArcBuilder(ArcNode arcNode)
    {

        ListExtrasScoper.ToDraw toDraw = new()
        {
            name = arcNode.Name,
            color = scope.Color.First(),
            figure = "ArcNode",
            points = new PointF[]
            {
                    (PointF)Visit(node: arcNode.Center, scope),
                    (PointF)Visit(node: arcNode.P1, scope),
                    (PointF)Visit(node: arcNode.P2, scope)
            },
            rad = (double)Visit(node: arcNode.Measure, scope),
            comment = (string)Visit(arcNode.Comment, scope)
        };
        return toDraw;

    }
    private ListExtrasScoper.ToDraw SegBuilder(SegmentNode segmentNode)
    {

        ListExtrasScoper.ToDraw toDraw = new()
        {
            name = segmentNode.Name,
            color = scope.Color.First(),
            figure = "SegmentNode",
            points = new PointF[]
            {
                (PointF)Visit(node: segmentNode.A, scope),
                (PointF)Visit(node: segmentNode.B, scope)
            },
            comment = null!
        };
        return toDraw;

    }
    private ListExtrasScoper.ToDraw RayBuilder(RayNode rayNode)
    {

        ListExtrasScoper.ToDraw toDraw = new()
        {
            name = rayNode.Name,
            color = scope.Color.First(),
            figure = "RayNode",
            points = new PointF[]
            {
                    (PointF)Visit(node: rayNode.P1, scope),
                    (PointF)Visit(node: rayNode.P2, scope)
            },
            comment = (string)Visit(rayNode.Comment, scope)
        };
        return toDraw;

    }
    private ListExtrasScoper.ToDraw LineBuilder(LineNode lineNode)
    {
        ListExtrasScoper.ToDraw toDraw = new()
        {
            name = lineNode.Name,
            color = scope.Color.First(),
            figure = "LineNode",
            points = new PointF[]
            {
                    (PointF)Visit(node: lineNode.A, scope),
                    (PointF)Visit(node: lineNode.B, scope)
            },
            comment = (string)Visit(lineNode.Comment, scope)

        };
        return toDraw;
    }
    private ListExtrasScoper.ToDraw CircleBuilder(CircleNode circleNode)
    {
        ListExtrasScoper.ToDraw toDraw = new()
        {
            name = circleNode.name,
            color = scope.Color.First(),
            figure = "CircleNode",
            points = new PointF[] { (PointF)Visit(node: circleNode.Center, scope) },
            rad = (double)Visit(node: circleNode.Radius, scope),
            comment = (string)Visit(circleNode.Comment, scope)
        };

        return toDraw;
    }
    private ListExtrasScoper.ToDraw PointBuilder(PointNode pointNode)
    {
        var x = (double)Visit(node: pointNode.X, scope);
        var y = (double)Visit(node: pointNode.Y, scope);

        ListExtrasScoper.ToDraw toDraw = new()
        {
            name = pointNode.Name,
            color = scope.Color.First(),
            figure = "PointNode",
            points = new PointF[] { new((float)x, (float)y) },
            comment = null!
        };
        scope.poiND.Add(pointNode.Name, toDraw.points[0]);
        return toDraw;
    }
    #endregion
}