using Tokens;
using Nodes;

namespace ParserAnalize;
public partial class Parser
{
    private List<Token> Tokens { get; set; }
    private static List<FunctionDeclarationNode> fDN = new();
    private int currentTokenIndex = 0;
    private readonly HashSet<string> ImportedFiles = new();
    private readonly Dictionary<string, Node> ImportedModules = new();
    private Token? CurrentToken => currentTokenIndex < Tokens.Count ? Tokens[currentTokenIndex] : null;

    /// <summary>
    /// Represents a parser that processes a list of tokens.
    /// </summary>
    /// <param name="tokens">The list of tokens to be processed.</param>
    public Parser(List<Token> tokens)
    {
        Tokens = tokens;
    }

    /// <summary>
    /// Consume a token in the parser.
    /// </summary>
    private Token ConsumeToken(TokenType type)
    {
        return CurrentToken?.Type == type
            ? Tokens[currentTokenIndex++]
            : throw new Exception($"Expected token {type}, but found {CurrentToken?.Type} at line {CurrentToken?.Line} and column {CurrentToken?.Column}");
    }
    /// <summary>
    /// Represents a node in the parse tree.
    /// </summary>
    /// <returns>The parsed node.</returns>
    public Node Parse()
    {
        return ParseExpression();
    }
    private Node ParseExpression()
    {
        Node left = ParseTerm();

        while (CurrentToken?.Type == TokenType.Operator && (CurrentToken.Value == "+" || CurrentToken.Value == "-"))
        {
            var operatorToken = CurrentToken;
            ConsumeToken(TokenType.Operator);
            Node right = ParseTerm();
            left = new BinaryExpressionNode(left, operatorToken.Value, right);
        }

        return left;
    }
    private Node ParseTerm()
    {
        Node left = ParseFactor();

        while (CurrentToken?.Type == TokenType.Operator && (CurrentToken.Value == "*" || CurrentToken.Value == "/"))
        {
            var operatorToken = CurrentToken;
            ConsumeToken(TokenType.Operator);
            Node right = ParseFactor();
            left = new BinaryExpressionNode(left, operatorToken.Value, right);
        }
        return left;
    }
    private Node ParseFactor()
    {
        Node left = ParsePrimary();

        while (CurrentToken?.Type == TokenType.Operator && CurrentToken.Value == "^")
        {
            var operatorToken = CurrentToken;
            ConsumeToken(TokenType.Operator);
            Node right = ParsePrimary();
            left = new BinaryExpressionNode(left, operatorToken.Value, right);
        }

        return left;
    }
    private Node ParsePrimary()
    {
        if (CurrentToken?.Type == TokenType.LParen)
        {
            ConsumeToken(TokenType.LParen);
            var node = ParseExpression();
            _ = ConsumeToken(TokenType.RParen);
            return node;
        }
        else if (CurrentToken?.Type == TokenType.Operator && CurrentToken?.Value == "-")
        {
            ConsumeToken(TokenType.Operator);
            var node = ParsePrimary();
            return new BinaryExpressionNode(new ValueNode(0.0), "-", node);
        }
        else if (CurrentToken?.Type == TokenType.Number)
        {
            var numberToken = CurrentToken;
            ConsumeToken(TokenType.Number);
            return new ValueNode(double.Parse(numberToken.Value));
        }
        else
        {
            return (CurrentToken?.Type) switch
            {
                TokenType.Figure => ParseFigure(),
                TokenType.DrawKeyword => ParseDraw,
                TokenType.MeasureKeyword => ParseMeasure,
                TokenType.FunctionKeyword => ParseFunctionDeclaration,
                TokenType.ColorKeyword => ParseColor,
                TokenType.RestoreKeyword => ParseRestore,
                TokenType.LetKeyword => ParseVariableDeclaration,
                TokenType.Point => ParsePointExpression,
                TokenType.IfKeyword => ParseIfExpression,
                TokenType.Number => ParseNumber,
                TokenType.StringLiteral => ParseStringLiteral,
                TokenType.Identifier => ParseIdentifier,
                TokenType.ImportKeyword => ParseImport,
                TokenType.IntersectKeyword => ParseIntersect,
                _ => throw new Exception($"Unexpected token {CurrentToken?.Type} at line {CurrentToken?.Line} and column {CurrentToken?.Column}"),
            };
        }
    }
}