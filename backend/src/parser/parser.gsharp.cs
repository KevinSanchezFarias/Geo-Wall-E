using Tokens;
using Nodes;
using Lists;
using EvaluatorAnalize;
using LexerAnalize;

namespace ParserAnalize;
public partial class Parser
{
    private readonly string[] predefinedFunctionIdentifiers =
    {
    "Sin",
    "Cos",
    "Tan",
    "Sqrt",
    "Pow",
    "Abs",
    "Floor",
    "Ceiling",
    "Round",
    "Truncate",
    "Log",
    "Log10",
    "Exp",
    "Min",
    "Max",
    "Sum",
    "Average",
    "Median",
    "Mode",
    "Range",
    "Fact",
    "Rand"
};
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

            return CurrentToken?.Type switch
            {
                TokenType.Operator when CurrentToken?.Value == "=" => ParseDeclareSimpleVar(token),
                TokenType.Comma when WhenToMultiAssign() => MultiAssignParse(token),
                TokenType.LParen => ParseFunctionCallParse(token),
                _ => new IdentifierNode(token.Value)
            };

            bool WhenToMultiAssign()
            {
                int peekIndex = currentTokenIndex + 1;

                // Check if there are enough tokens left for a multi-assignment
                if (peekIndex >= Tokens.Count)
                {
                    return false;
                }

                // Check if the next token is an identifier
                if (Tokens[peekIndex]?.Type != TokenType.Identifier)
                {
                    return false;
                }

                // Skip over identifiers and commas
                while (peekIndex < Tokens.Count && (Tokens[peekIndex]?.Type == TokenType.Identifier || Tokens[peekIndex]?.Type == TokenType.Comma))
                {
                    peekIndex++;
                }

                // Check if the token after the identifiers and commas is an '='
                return peekIndex < Tokens.Count && Tokens[peekIndex]?.Type == TokenType.Operator && Tokens[peekIndex]?.Value == "=";
            }
        }
    }
    private Node ParseDeclareSimpleVar(Token token)
    {
        _ = ConsumeToken(TokenType.Operator);
        if (CurrentToken?.Type == TokenType.LBrace)
        {
            return ParseSequence(token.Value);
        }
        Node expression = ParseExpression();
        return new ConstDeclarationNode(token.Value, expression);
    }

    private Node ParseFunctionCallParse(Token functionName)
    {
        _ = ConsumeToken(TokenType.LParen);
        var args = new List<Node>();
        while (CurrentToken?.Type != TokenType.RParen)
        {
            args.Add(ParseExpression());
            if (CurrentToken?.Type == TokenType.Comma) { _ = ConsumeToken(TokenType.Comma); }
        }
        _ = ConsumeToken(TokenType.RParen);


        // If the next token is a right parenthesis, it's a function call
        if (CurrentToken?.Type == TokenType.Operator && CurrentToken?.Value == "=")
        {
            _ = ConsumeToken(TokenType.Operator);
            var body = ParseExpression();
            return new FunctionDeclarationNode(functionName.Value, args, body);
        }
        else
        {
            // Check if the function name matches a predefined function
            if (predefinedFunctionIdentifiers.Contains(functionName.Value))
            {
                return new FunctionPredefinedNode(functionName.Value, args);
            }
            else
            {
                return new FunctionCallNode(functionName.Value, args);
            }
        }
    }
    private Node MultiAssignParse(Token token)
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
    private Node ParseVariableDeclaration
    {
        get
        {
            _ = ConsumeToken(TokenType.LetKeyword);
            var identifier = ConsumeToken(TokenType.Identifier);
            _ = ConsumeToken(TokenType.Operator);
            var value = ParseExpression();
            if (CurrentToken?.Type == TokenType.EOL)
            {
                _ = ConsumeToken(TokenType.EOL);
                return ParseMultiLet(identifier, value);
            }
            else
            {
                _ = ConsumeToken(TokenType.InKeyword);
                var body = ParseExpression();
                return new VariableDeclarationNode(new ValueNode(identifier), value, body);
            }
        }

    }

    private Node ParseMultiLet(Token identifier, Node value)
    {
        var variableDeclarations = new List<ConstDeclarationNode>
        {
            // Add the first expression to the list
            new(identifier.Value, value)
        };

        while (CurrentToken?.Type != TokenType.InKeyword)
        {
            // Parse the next expression
            var nextIdentifier = ConsumeToken(TokenType.Identifier);
            _ = ConsumeToken(TokenType.Operator); // consume the '=' operator
            var nextValue = ParseExpression();

            // Add the expression to the list
            variableDeclarations.Add(new ConstDeclarationNode(nextIdentifier.Value, nextValue));

            if (CurrentToken?.Type == TokenType.EOL)
            {
                _ = ConsumeToken(TokenType.EOL);
            }
        }

        _ = ConsumeToken(TokenType.InKeyword);
        var body = ParseExpression();

        // Create the MultipleVariableDeclarationNode with the parsed variable declarations and body
        var multiLetNode = new MultipleVariableDeclarationNode(variableDeclarations, body);

        return multiLetNode;
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
    private Node ParseImport
    {
        get
        {
            /* _ = ConsumeToken(TokenType.ImportKeyword);
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
            ImportedModules[path.Value] = result; */
            return new EndNode();
        }
    }

    #endregion
}