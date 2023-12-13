using Tokens;

namespace LexerAnalize;
public partial class Lexer
{
    private string Text { get; set; }
    private static int Position { get; set; }
    private int Column { get; set; }
    private int Line { get; set; }
    private char CurrentChar { get; set; }

    public Lexer(string input)
    {
        Text = input;
        Position = 0;
        Column = 1;
        Line = 1;
        CurrentChar = input[Position];
        Tokenize();
    }

    private Token StringLiteral()
    {
        string result = "";
        Advance();
        while (CurrentChar != '\0' && CurrentChar != '"')
        {
            result += CurrentChar;
            Advance();
        }
        if (CurrentChar == '\0')
        {
            throw new Exception($"Unterminated string literal at line {Line} and column {Column}");
        }
        Advance(); // Consume the second '"' to advance to the next token
        return new Token(TokenType.StringLiteral, result, Line, Column);
    }

    private void DeclareFunction()
    {
        LexTokens.Add(new Token(TokenType.FunctionKeyword, "function", Line, Column));
        Advance();
        SkipWhitespace();
        var functionName = "";
        while (CurrentChar != '\0' && char.IsLetterOrDigit(CurrentChar))
        {
            functionName += CurrentChar;
            Advance();
        }
        LexTokens.Add(new Token(TokenType.FIdentifier, functionName, Line, Column));
        if (CurrentChar != '(')
        {
            throw new Exception($"Expected '(' after function name, but found '{CurrentChar}' at line {Line} and column {Column}");
        }
        Advance();
        while (CurrentChar != '\0' && CurrentChar != ')')
        {
            SkipWhitespace();
            var parameter = "";
            while (CurrentChar != '\0' && char.IsLetterOrDigit(CurrentChar))
            {
                parameter += CurrentChar;
                Advance();
            }
            LexTokens.Add(new Token(TokenType.Parameter, parameter, Line, Column));
            SkipWhitespace();
            if (CurrentChar == ',')
            {
                Advance();
            }
        }
        if (CurrentChar != ')')
        {
            throw new Exception($"Expected ')' after function parameters, but found '{CurrentChar}' at line {Line} and column {Column}");
        }
        Advance();
        SkipWhitespace();
        if (CurrentChar != '=' && Peek() != '>')
        {
            throw new Exception($"Expected '=>' after function declaration, but found '{CurrentChar}' at line {Line} and column {Column}");
        }
        LexTokens.Add(new Token(TokenType.Flinq, "=>", Line, Column));
        Advance(); Advance();
        SkipWhitespace();
        Tokenize();
    }

    private Token Number()
    {
        string result = "";
        if (CurrentChar == '-')
        {
            result += CurrentChar;
            Advance();
        }
        while (CurrentChar != '\0' && char.IsDigit(CurrentChar))
        {
            result += CurrentChar;
            Advance();
        }
        if (CurrentChar == '.')
        {
            result += CurrentChar;
            Advance();
            while (CurrentChar != '\0' && char.IsDigit(CurrentChar))
            {
                result += CurrentChar;
                Advance();
            }
        }
        // Return the number token
        return new Token(TokenType.Number, result, Line, Column);
    }

    private Token Keyword()
    {
        string result = "";
        while (CurrentChar != '\0' && char.IsLetterOrDigit(CurrentChar))
        {
            result += CurrentChar;
            Advance();
        }
        return result switch
        {
            "intersect" => new Token(TokenType.IntersectKeyword, result, Line, Column),
            "draw" => new Token(TokenType.DrawKeyword, result, Line, Column),
            "measure" => new Token(TokenType.MeasureKeyword, result, Line, Column),
            "point" => new Token(TokenType.Point, result, Line, Column),
            "line" => new Token(TokenType.Figure, result, Line, Column),
            "segment" => new Token(TokenType.Figure, result, Line, Column),
            "ray" => new Token(TokenType.Figure, result, Line, Column),
            "circle" => new Token(TokenType.Figure, result, Line, Column),
            "arc" => new Token(TokenType.Figure, result, Line, Column),
            "color" => new Token(TokenType.ColorKeyword, result, Line, Column),
            "restore" => new Token(TokenType.RestoreKeyword, result, Line, Column),
            "const" => new Token(TokenType.Const, result, Line, Column),
            "flinq" => new Token(TokenType.Flinq, result, Line, Column),
            "llinq" => new Token(TokenType.LLinq, result, Line, Column),
            "let" => new Token(TokenType.LetKeyword, result, Line, Column),
            "function" => new Token(TokenType.FunctionKeyword, result, Line, Column),
            "if" => new Token(TokenType.IfKeyword, result, Line, Column),
            "then" => new Token(TokenType.ThenKeyword, result, Line, Column),
            "else" => new Token(TokenType.ElseKeyword, result, Line, Column),
            "in" => new Token(TokenType.InKeyword, result, Line, Column),
            _ => new Token(TokenType.Identifier, result, Line, Column),
        };
    }
    /// <summary>
    /// Advances the position of the lexer to the next character in the input text.
    /// </summary>
    private void Advance()
    {
        Position++;
        Column++;
        if (Position > Text.Length - 1)
        {
            CurrentChar = '\0';
        }
        else
        {
            CurrentChar = Text[Position];
        }
    }
    /// <summary>
    /// Skips all whitespace characters until a non-whitespace character is found.
    /// </summary>
    private void SkipWhitespace()
    {
        while (CurrentChar != '\0' && char.IsWhiteSpace(CurrentChar))
        {
            Advance();
        }
    }
    /// <summary>
    /// Returns the next character in the input string without consuming it.
    /// </summary>
    /// <returns>The next character in the input string, or '\0' if there are no more characters.</returns>
    public char Peek()
    {
        var peek_pos = Position + 1;
        return peek_pos >= Text.Length ? '\0' : Text[peek_pos];
    }
}