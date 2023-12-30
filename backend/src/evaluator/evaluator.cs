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
    public Evaluator(Node ast)
    {
        AST = ast;
    }
    private Dictionary<string, object> variables = new();
    private readonly Stack<Dictionary<string, object>> scopes = new();
    /// <summary>
    /// Evaluates the abstract syntax tree (AST) and returns the result.
    /// </summary>
    /// <returns>The result of the evaluation.</returns>
    public object Evaluate()
    {
        return Visit(AST);
    }
    /// <summary>
    /// Draws the specified <see cref="DrawNode"/> and returns a list of <see cref="ToDraw"/> objects.
    /// </summary>
    /// <param name="node">The <see cref="DrawNode"/> to be drawn.</param>
    /// <returns>A list of <see cref="ToDraw"/> objects representing the figures to be drawn.</returns>
    public object Draw(DrawNode drawNode)
    {
        if (drawNode.decl)
        {
            // Handle the case where the circle is declared and drawn in the same statement
            return drawNode.Figures switch
            {
                CircleNode circleNode => CircleBuilder(circleNode),
                LineNode lineNode => LineBuilder(lineNode),
                SegmentNode segmentNode => SegBuilder(segmentNode),
                RayNode rayNode => RayBuilder(rayNode),
                ArcNode arcNode => ArcBuilder(arcNode),
                PointNode pointNode => PointBuilder(pointNode),
                _ => null!,
            };
        }
        else
        {
            IdentifierNode x = (IdentifierNode)Visit(new ValueNode(drawNode.Figures));
            if (LE.toDraws.Any(p => x.Identifier == p.name))
            {
                return LE.toDraws.First(p => x.Identifier == p.name);
            }
            return null!;
        }
    }
    /// <summary>
    /// Visits a given node and performs the corresponding evaluation logic based on the node type.
    /// </summary>
    /// <param name="node">The node to visit.</param>
    /// <returns>The result of the evaluation.</returns>
    private object Visit(Node node)
    {
        switch (node)
        {
            case EndNode:
                return null!;
            case ValueNode valueNode:
                return valueNode.Value;
            case MultiAssignmentNode multiAssignmentNode:
                return MultiAssignmentHandler(multiAssignmentNode);
            case ConstDeclarationNode constDeclarationNode:
                return ConstDeclarationNodeHandler(constDeclarationNode);
            case GlobalConstNode globalConstNode:
                return GlobalConstNodeHandler(globalConstNode);
            case PointNode pointNode:
                return PointNodeHandler(pointNode);
            /* case IntersectNode intersectNode:
                return IntersectHandler(intersectNode); */
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
            case Figure figure:
                return FigureHandler(figure);
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
        #endregion
    }

    private object MeasureNodeHandler(MeasureNode measureNode)
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

    /// <summary>
    /// Handles the given figure and returns the result.
    /// </summary>
    /// <param name="figure">The figure to be handled.</param>
    /// <returns>The result of handling the figure.</returns>
    private object FigureHandler(Figure figure)
    {
        return figure switch
        {
            CircleNode circleNode => CircleNHandler(circleNode),
            LineNode lineNode => LineNHandler(lineNode),
            SegmentNode segmentNode => SegmentNHandler(segmentNode),
            RayNode rayNode => RayNHandler(rayNode),
            ArcNode arcNode => ArcNHandler(arcNode),
            _ => throw new Exception($"Unexpected node type {figure.GetType()}"),
        };
    }
    /// <summary>
    /// Handles the ArcNode object and adds it to the drawing list.
    /// </summary>
    /// <param name="arcNode">The ArcNode object to handle.</param>
    /// <returns>Returns null.</returns>
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
    /// <summary>
    /// Handles the evaluation of a RayNode object.
    /// </summary>
    /// <param name="rayNode">The RayNode object to be evaluated.</param>
    /// <returns>Returns null.</returns>
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
    /// <summary>
    /// Handles the evaluation of a SegmentNode object.
    /// </summary>
    /// <param name="segmentNode">The SegmentNode object to be evaluated.</param>
    /// <returns>Returns null.</returns>
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
    /// <summary>
    /// Handles the LineNode and adds the corresponding drawing information to the toDraws collection.
    /// </summary>
    /// <param name="lineNode">The LineNode to be handled.</param>
    /// <returns>Returns null.</returns>
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
    /// <summary>
    /// Handles the evaluation of a CircleNode object.
    /// </summary>
    /// <param name="circleNode">The CircleNode object to be evaluated.</param>
    /// <returns>Returns null.</returns>
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
    private LE.ToDraw CircleBuilder(CircleNode circleNode)
    {
        LE.ToDraw toDraw = new()
        {
            name = circleNode.name,
            color = LE.Color.First(),
            figure = "CircleNode",
            points = new PointF[] { (PointF)Visit(node: circleNode.Center) },
            rad = (double)Visit(node: circleNode.Radius),
            comment = null!,
        };
        return toDraw;
    }
    /// <summary>
    /// Handles a PointNode object.
    /// </summary>
    /// <param name="pointNode">The PointNode object to handle.</param>
    /// <returns>Returns null.</returns>
    private object PointNodeHandler(PointNode pointNode)
    {
        if (pointNode.X is null || pointNode.Y is null)
        {
            LE.poiND.Add(pointNode.Name, new PointF(new Random().Next(150, 300), new Random().Next(150, 300)));
            return null!;
        }
        else
        {
            var point = PointBuilder(pointNode);
            LE.poiND.Add(pointNode.Name, point.Item2); // Add the PointF to the dictionary
            return null!;
        }
    }
    private (string, PointF) PointBuilder(PointNode pointNode)
    {
        return (pointNode.Name, new PointF(Convert.ToSingle(Visit(node: pointNode.X)), Convert.ToSingle(Visit(node: pointNode.Y))));
    }
    private object GlobalConstNodeHandler(GlobalConstNode globalConstNode)
    {
        var matchingNode = LE.DeclaredConst.FirstOrDefault(node => node.Identifier == globalConstNode.Identifier);
        return Visit((Node)matchingNode!.Value);
    }
    private object MultiAssignmentHandler(MultiAssignmentNode multiAssignmentNode)
    {
        var identifiers = multiAssignmentNode.Identifiers;
        var sequence = multiAssignmentNode.Sequence;

        // Evaluate the sequence
        var values = sequence.Nodes.Select(Visit).ToList();

        // Assign each value to its corresponding identifier, or UndefinedNode.Value if there is no corresponding value
        for (int i = 0; i < identifiers.Count - 1; i++)
        {
            var identifier = identifiers[i];
            var value = i < values.Count ? values[i] : UndefinedNode.Value;

            // Create a new ConstDeclarationNode and add it to cDN
            var constDeclarationNode = new ConstDeclarationNode(identifier, new ValueNode(value));
            LE.cDN.Add(constDeclarationNode);
        }

        // If there are as many or more values than identifiers, create a new SequenceNode for the last identifier
        if (values.Count >= identifiers.Count)
        {
            var lastIdentifier = identifiers.Last();
            var remainingValues = values.Skip(identifiers.Count - 1).Select(value => new ValueNode(value)).ToList<Node>();
            var sequenceNode = new SequenceNode(remainingValues, lastIdentifier);
            LE.Seqs.Add(sequenceNode);
        }
        // Otherwise, create a new ConstDeclarationNode for the last identifier and add it to cDN
        else
        {
            var lastIdentifier = identifiers.Last();
            var value = UndefinedNode.Value;
            var constDeclarationNode = new ConstDeclarationNode(lastIdentifier, new ValueNode(value));
            LE.cDN.Add(constDeclarationNode);
        }

        return null!;
    }
    /// <summary>
    /// Handles the evaluation of a constant declaration node.
    /// </summary>
    /// <param name="constDeclarationNode">The constant declaration node to be evaluated.</param>
    /// <returns>Returns null.</returns>
    private object ConstDeclarationNodeHandler(ConstDeclarationNode constDeclarationNode)
    {
        if (constDeclarationNode.Value is ValueNode valueNode)
        {
            variables[constDeclarationNode.Identifier] = valueNode.Value;
        }
        else if (constDeclarationNode.Value is Figure figure)
        {
            variables[constDeclarationNode.Identifier] = figure;
            return null!;
        }
        else
        {
            variables[constDeclarationNode.Identifier] = Visit(constDeclarationNode.Value);
        }
        LE.DeclaredConst.Add(new GlobalConstNode(
            constDeclarationNode.Identifier,
            new ValueNode(Visit(constDeclarationNode.Value))));
        return null!;
    }
}