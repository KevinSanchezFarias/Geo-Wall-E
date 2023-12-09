using Tokens;
using Nodes;

namespace ParserAnalize;
public class Parser(List<Token> tokens)
{
    private static List<FunctionDeclarationNode> fDN = [];
    public static readonly List<ConstDeclarationNode> cDN =
    [
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
    ];
    private List<Token> Tokens { get; set; } = tokens;
    private int currentTokenIndex = 0;

    private Token? CurrentToken => currentTokenIndex < Tokens.Count ? Tokens[currentTokenIndex] : null;

    private Token ConsumeToken(TokenType type)
    {
        if (CurrentToken?.Type == type)
        {
            return Tokens[currentTokenIndex++];
        }
        else
        {
            throw new Exception($"Expected token {type}, but found {CurrentToken?.Type} at line {CurrentToken?.Line} and column {CurrentToken?.Column}");
        }
    }
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
                TokenType.FunctionKeyword => ParseFunctionDeclaration,
                TokenType.Const => ParseConstDeclaration,
                TokenType.LetKeyword => ParseVariableDeclaration(),
                TokenType.Point => ParsePointExpression,
                TokenType.IfKeyword => ParseIfExpression,
                TokenType.Number => ParseNumber,
                TokenType.StringLiteral => ParseStringLiteral,
                TokenType.Identifier => ParseIdentifier,
                _ => throw new Exception($"Unexpected token {CurrentToken?.Type} at line {CurrentToken?.Line} and column {CurrentToken?.Column}"),
            };
        }
    }
    private Node ParseNumber
    {
        get
        {
            var token = ConsumeToken(TokenType.Number);
            return new ValueNode(double.Parse(token.Value));
        }
    }

    private Node ParseIdentifier
    {
        get
        {
            var token = ConsumeToken(TokenType.Identifier);

            // If the identifier is followed by a left parenthesis, treat it as a function call
            if (CurrentToken?.Type == TokenType.LParen)
            {
                // Parse a function call
                var args = new List<Node>();
                _ = ConsumeToken(TokenType.LParen);
                while (CurrentToken?.Type != TokenType.RParen)
                {
                    args.Add(ParseExpression());
                    if (CurrentToken?.Type == TokenType.Comma)
                    {
                        _ = ConsumeToken(TokenType.Comma);
                    }
                }
                _ = ConsumeToken(TokenType.RParen);
                // Check if the function name is in the list of predefined functions
                if (EvaluatorAnalize.Evaluator.predefinedFunctions.ContainsKey(token.Value))
                {
                    return new FunctionPredefinedNode(token.Value, args);
                }
                // Check if the function name is in the list of declared functions
                else if (fDN.Any(f => f.Name == token.Value))
                {
                    return new FunctionCallNode(token.Value, args);
                }
                else
                {
                    throw new Exception($"Undefined function {token.Value}");
                }
            }
            else
            {
                // Parse an identifier
                return new IdentifierNode(token.Value);
            }
        }
    }
    private Node ParseVariableDeclaration()
    {
        _ = ConsumeToken(TokenType.LetKeyword);

        if (CurrentToken?.Type == TokenType.LLinq)
        {
            return ParseMultipleVariableDeclaration;
        }
        else
        {
            return ParseSingleVariableDeclaration;
        }
    }

    private Node ParseSingleVariableDeclaration
    {
        get
        {
            var identifier = ConsumeToken(TokenType.Identifier);
            _ = ConsumeToken(TokenType.Operator);
            var value = ParseExpression();
            _ = ConsumeToken(TokenType.InKeyword);
            var body = ParseExpression();
            return new VariableDeclarationNode(identifier.Value, value, body);
        }
    }

    private Node ParseMultipleVariableDeclaration
    {
        get
        {
            _ = ConsumeToken(TokenType.LLinq);
            _ = ConsumeToken(TokenType.LBrace);

            var declarations = new List<VariableDeclarationNode>();

            while (CurrentToken?.Type != TokenType.RBrace)
            {
                var identifier = ConsumeToken(TokenType.Identifier);
                _ = ConsumeToken(TokenType.Operator);
                var value = ParseExpression();
                declarations.Add(new VariableDeclarationNode(identifier.Value, value, null!));

                if (CurrentToken?.Type == TokenType.Comma)
                {
                    _ = ConsumeToken(TokenType.Comma);
                }
            }

            _ = ConsumeToken(TokenType.RBrace);
            _ = ConsumeToken(TokenType.InKeyword);
            var body = ParseExpression();

            return new MultipleVariableDeclarationNode(declarations, body);
        }
    }

    private Node ParsePointExpression
    {
        get
        {
            var left = ParseExpression();
            var operatorToken = ConsumeToken(TokenType.Point);
            var right = ParseExpression();
            return new BinaryExpressionNode(left, operatorToken.Value, right);
        }
    }

    private Node ParseIfExpression
    {
        get
        {
            _ = ConsumeToken(TokenType.IfKeyword);
            var condition = ParseCondition;
            _ = ConsumeToken(TokenType.ThenKeyword);
            var thenExpression = ParseExpression();
            _ = ConsumeToken(TokenType.ElseKeyword);
            var elseExpression = ParseExpression();
            return new IfExpressionNode(condition, thenExpression, elseExpression);
        }
    }

    private Node ParseCondition
    {
        get
        {
            _ = ConsumeToken(TokenType.LParen);
            var left = ParseExpression();
            var operatorToken = ConsumeToken(TokenType.ComparisonOperator);
            var right = ParseExpression();
            _ = ConsumeToken(TokenType.RParen);
            return new BinaryExpressionNode(left, operatorToken.Value, right);
        }
    }

    private Node ParseStringLiteral
    {
        get
        {
            var token = ConsumeToken(TokenType.StringLiteral);
            return new ValueNode(token.Value);
        }
    }

    private Node ParseFunctionDeclaration
    {
        get
        {
            _ = ConsumeToken(TokenType.FunctionKeyword);
            var name = ConsumeToken(TokenType.FIdentifier);
            var args = new List<string>();
            while (CurrentToken?.Type != TokenType.Flinq)
            {
                var arg = ConsumeToken(TokenType.Parameter);
                args.Add(arg.Value);
            }
            _ = ConsumeToken(TokenType.Flinq);
            var body = ParseExpression();
            FDN.Add(new FunctionDeclarationNode(name.Value, args, body));
            return new EndNode();
        }
    }

    private Node ParseConstDeclaration
    {
        get
        {
            _ = ConsumeToken(TokenType.Const);
            var name = ConsumeToken(TokenType.Identifier);
            _ = ConsumeToken(TokenType.Operator);
            var valueNode = ParseExpression();

            cDN.Insert(0, new ConstDeclarationNode(name.Value, valueNode)); // Store the valueNode at the beginning of the list
            return new EndNode();
        }
    }
    public static List<FunctionDeclarationNode> FDN { get => fDN; set => fDN = value; }
}