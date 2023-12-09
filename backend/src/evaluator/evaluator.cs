using Nodes;
using ParserAnalize;

namespace EvaluatorAnalize;
public class Evaluator(Node ast)
{
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
    private Node AST { get; set; } = ast;
    private Dictionary<string, object> variables = [];

    public object Evaluate()
    {
        return Visit(AST);
    }

    private object Visit(Node node)
    {

        switch (node)
        {
            case EndNode:
                return null!;
            case ValueNode valueNode:
                return valueNode.Value;
            case FunctionCallNode functionCallNode:
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
            case FunctionPredefinedNode functionPredefinedNode:
                if (predefinedFunctions.TryGetValue(functionPredefinedNode.Name, out var function))
                {
                    var argValues = functionPredefinedNode.Args.Select(arg => (double)Visit(arg)).ToArray();
                    return function(argValues);
                }
                else
                {
                    throw new Exception($"Undefined function {functionPredefinedNode.Name}");
                }
            case BinaryExpressionNode binaryExpressionNode:
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
                else if (binaryExpressionNode.Operator == "!=" && left is string leftStr3 && right is string rightStr3)
                {
                    return leftStr3 != rightStr3;
                }
                else if (left is double leftNum && right is double rightNum)
                {
                    return binaryExpressionNode.Operator switch
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
                    };
                }
                else
                {
                    throw new Exception($"Invalid operands for operator {binaryExpressionNode.Operator}");
                }
            case IfExpressionNode ifExpressionNode:
                var condition = Visit(ifExpressionNode.Condition);
                if (condition is bool conditionBool)
                {
                    if (conditionBool)
                    {
                        return Visit(ifExpressionNode.ThenBody);
                    }
                    else
                    {
                        return Visit(ifExpressionNode.ElseBody);
                    }
                }
                else
                {
                    throw new Exception($"Invalid condition type {condition.GetType()}");
                }
            case IdentifierNode identifierNode:
                if (variables.TryGetValue(identifierNode.Identifier, out var value))
                {
                    return value;
                }
                else if (Parser.cDN.Any(c => c.Identifier == identifierNode.Identifier))
                {
                    if (Parser.cDN.First(c => c.Identifier == identifierNode.Identifier).Value is ValueNode vnc)
                    {
                        return vnc.Value;
                    }
                    return Parser.cDN.First(c => c.Identifier == identifierNode.Identifier).Value;
                }
                else
                {
                    throw new Exception($"Undefined variable {identifierNode.Identifier}");
                }
            case VariableDeclarationNode varDecl:
                variables[varDecl.Identifier] = Visit(varDecl.Value);
                return Visit(varDecl.Body);
            case MultipleVariableDeclarationNode multipleVarDecl:
                foreach (var varDecl in multipleVarDecl.Declarations)
                {
                    variables[varDecl.Identifier] = Visit(varDecl.Value);
                }
                return Visit(multipleVarDecl.Body);
            default:
                throw new Exception($"Unexpected node type {node.GetType()}");
        }
    }
}