using Tokens;
using Nodes;
using Lists;
using EvaluatorAnalize;
using LexerAnalize;

namespace ParserAnalize;
public partial class Parser
{
    #region Gsharp
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

            // First, check if the identifier already exists and return its value
            Node? existingIdentifier = token.Value switch
            {
                var value when LE.Seqs.Any(s => s.Identifier == value) => LE.Seqs.First(s => s.Identifier == value),
                var value when LE.cDN.Any(p => p.Identifier == value) => LE.cDN.First(p => p.Identifier == value),
                var value when LE.DeclaredConst.Any(p => p.Identifier == value) => LE.DeclaredConst.First(p => p.Identifier == value),
                var value when LE.poiND.Any(p => p.Key == value) => new IdentifierNode(token.Value),
                _ => null
            };
            if (existingIdentifier != null)
            {
                return existingIdentifier!;
            }
            // If the identifier doesn't exist, try to declare it
            return CurrentToken?.Type switch
            {
                TokenType.Operator when CurrentToken?.Value == "=" => ParseDeclareSimpleVar(token),
                TokenType.Comma => MultiAssignParse(token),
                TokenType.LParen => FunctionCallParse(token),
                _ => new IdentifierNode(token.Value)
            };
            Node MultiAssignParse(Token token)
            {
                // Parse multiple assignments
                var identifiers = new List<string> { token.Value };
                while (CurrentToken?.Type == TokenType.Comma)
                {
                    _ = ConsumeToken(TokenType.Comma);
                    identifiers.Add(ConsumeToken(TokenType.Identifier).Value);
                }
                _ = ConsumeToken(TokenType.Operator);
                Node sequence = ParseExpression();
                return new MultiAssignmentNode(identifiers, sequence);
            }
            Node FunctionCallParse(Token token)
            {
                // Parse a function call
                var args = new List<Node>();
                _ = ConsumeToken(TokenType.LParen);
                while (CurrentToken?.Type != TokenType.RParen)
                {
                    args.Add(ParseExpression());
                    if (CurrentToken?.Type == TokenType.Comma) { _ = ConsumeToken(TokenType.Comma); }
                }
                _ = ConsumeToken(TokenType.RParen);
                // Check if the function name is in the list of predefined functions
                return LE.predefinedFunctions.ContainsKey(token.Value)
                    ? new FunctionPredefinedNode(token.Value, args)
                    : FDN.Any(f => f.Name == token.Value)
                        ? (Node)new FunctionCallNode(token.Value, args)
                        : throw new Exception($"Undefined function {token.Value}");
            }
            Node ParseDeclareSimpleVar(Token token)
            {
                _ = ConsumeToken(TokenType.Operator);
                if (CurrentToken?.Type == TokenType.LBrace)
                {
                    return ParseSequence(token.Value);
                }
                Node expression = ParseExpression();
                return new ConstDeclarationNode(token.Value, expression);
            }
        }
    }
    private Node ParseVariableDeclaration()
    {
        _ = ConsumeToken(TokenType.LetKeyword);

        return CurrentToken?.Type == TokenType.LLinq ? ParseMultipleVariableDeclaration : ParseSingleVariableDeclaration;
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
            var left = ParseBinaryExpression();
            var operatorToken = ConsumeToken(TokenType.ComparisonOperator);
            var right = ParseExpression();
            _ = ConsumeToken(TokenType.RParen);
            return new BinaryExpressionNode(left, operatorToken.Value, right);
        }
    }
    private Node ParseBinaryExpression()
    {
        var left = ParseExpression();
        if (CurrentToken?.Type == TokenType.Operator)
        {
            var operatorToken = ConsumeToken(TokenType.Operator);
            var right = ParseExpression();
            return new BinaryExpressionNode(left, operatorToken.Value, right);
        }
        return left;
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
    private Node ParseSequence(string name)
    {
        _ = ConsumeToken(TokenType.LBrace);

        var values = new List<Node>();
        Node firstValue = null!;

        while (CurrentToken?.Type != TokenType.RBrace)
        {
            Node valueNode;
            if (CurrentToken?.Type == TokenType.IntersectKeyword)
            {
                return null!;
            }
            else if (CurrentToken?.Type == TokenType.DotDotDot)
            {
                // Handle the "..." syntax
                _ = ConsumeToken(TokenType.DotDotDot);
                if (CurrentToken?.Type == TokenType.RBrace)
                {
                    // If there's nothing after the "...", generate an infinite sequence of natural numbers
                    // This shit down here is a fvcking monster
                    // values.Add(new InfiniteSequenceNode(InfiniteSequence(1), name));
                    Task.Run(() =>
                    {
                        // If there's nothing after the "...", generate an infinite sequence of natural numbers
                        for (int i = 1; i < 2000000000; i++)
                        {
                            values.Add(new ValueNode(i));
                        }
                        //
                        values.RemoveAt(0);
                        MessageBox.Show($"THE SEQUENCE HAS MORE THAN 2 BILLIONS OF NUMBERS! at column {CurrentToken?.Column} btw");
                    });
                }
                else
                {
                    // If there's a number after the "...", generate a sequence of numbers up to that number
                    int end = int.Parse(ConsumeToken(TokenType.Number).Value);
                    for (double i = (double)((ValueNode)firstValue).Value; i <= end; i++)
                    {
                        values.Add(new ValueNode(i));
                    }
                    values.RemoveAt(0);
                }
                break;
            }
            else
            {
                valueNode = ParseExpression();
            }

            // If this is the first value, store it for later comparison
            firstValue ??= valueNode;

            values.Add(valueNode);

            if (CurrentToken?.Type == TokenType.Comma)
            {
                _ = ConsumeToken(TokenType.Comma);
            }
            else if (CurrentToken?.Type != TokenType.RBrace && CurrentToken?.Type != TokenType.DotDotDot)
            {
                throw new Exception($"Expected ',' or '}}' or '...' at column {CurrentToken?.Column}");
            }
        }

        _ = ConsumeToken(TokenType.RBrace);

        // Return a new SequenceNode
        return new SequenceNode(values, name);
    }
    public static List<FunctionDeclarationNode> FDN { get => fDN; set => fDN = value; }
    private Node ParseImport
    {
        get
        {
            _ = ConsumeToken(TokenType.ImportKeyword);
            var path = ConsumeToken(TokenType.StringLiteral);

            // Check if the file has already been imported
            if (ImportedFiles.Contains(path.Value))
            {
                // Skip this file
                return null!;
            }
            // Mark the file as imported
            ImportedFiles.Add(path.Value);
            // Lex, parse, and evaluate the file
            var lexerX = new Lexer(File.ReadAllText(path.Value));
            lexerX.LexTokens.RemoveAt(lexerX.LexTokens.Count - 1);
            var parser = new Parser(lexerX.LexTokens);
            var result = parser.Parse();
            Evaluator ev = new(parser.Parse());
            ev.Evaluate();

            // Store the result in a dictionary
            ImportedModules[path.Value] = result;
            return new EndNode();
        }
    }
    #endregion
}