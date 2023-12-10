using Nodes;
using ParserAnalize;

namespace EvaluatorAnalize;

public struct ToDraw
{
    public string figure;
    public string color;
    public Point[] points;
    public double rad;
    public string comment;


}
public class Evaluator(Node ast)
{

    private Node AST { get; set; } = ast;
    private Dictionary<string, object> variables = [];

    public object Evaluate()
    {
        return Visit(AST);
    }
    public object Draw(DrawNode node)
    {
        var points = node.Figures switch
        {
            CircleNode cNode => [new Point((int)cNode.Center.X, (int)cNode.Center.Y)],
            LineNode lineNode => [new Point((int)lineNode.A.X, (int)lineNode.A.Y), new Point((int)lineNode.B.X, (int)lineNode.B.Y)],
            SegmentNode segmentNode => [new Point((int)segmentNode.A.X, (int)segmentNode.A.Y), new Point((int)segmentNode.B.X, (int)segmentNode.B.Y)],
            RayNode rayNode => [new Point((int)rayNode.P1.X, (int)rayNode.P1.Y), new Point((int)rayNode.P2.X, (int)rayNode.P2.Y)],
            ArcNode arcNode => new List<Point> { new Point((int)arcNode.P1.X, (int)arcNode.P1.Y), new Point((int)arcNode.P2.X, (int)arcNode.P2.Y), new Point((int)arcNode.P3.X, (int)arcNode.P3.Y) },
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
            color = LE.Color,
            figure = node.Figures.GetType().Name,
            points = [.. points],
            rad = node.Figures is CircleNode circleNode ? (double)Visit(circleNode.Radius) : 0,
            comment = comment?.Value.ToString() ?? ""
        };
        return toDraw;
    }

    private object Visit(Node node)
    {
        switch (node)
        {
            case EndNode:
                return null!;
            case ValueNode valueNode:
                return valueNode.Value;
            case DrawNode drawNode:
                return Draw(drawNode);
            case FunctionCallNode functionCallNode:
                return InvokeDeclaredFunctionsHandler(functionCallNode);
            case FunctionPredefinedNode functionPredefinedNode:
                return FunctionPredefinedHandler(functionPredefinedNode);
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
                return LE.cDN.First(c => c.Identifier == identifierNode.Identifier).Value is ValueNode vnc
                    ? vnc.Value
                    : LE.cDN.First(c => c.Identifier == identifierNode.Identifier).Value;
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
}