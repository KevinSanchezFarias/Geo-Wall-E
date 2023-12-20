using Tokens;
namespace LexerAnalize;

public partial class Lexer
{
    public readonly List<Token> LexTokens = new();
    public void Tokenize()
    {
        while (CurrentChar != '\0')
        {
            switch (CurrentChar)
            {
                case '\r':
                    Advance();
                    Line++;
                    Column = 1;
                    continue;
                case '\n':
                    Advance();
                    Line++;
                    Column = 1;
                    continue;
                case char c when char.IsWhiteSpace(c):
                    SkipWhitespace();
                    continue;
                case char c when char.IsDigit(c):
                    LexTokens.Add(Number());
                    break;
                case char c when char.IsLetter(c):
                    var token = Keyword();
                    if (token.Type == TokenType.FunctionKeyword)
                    {
                        DeclareFunction();
                    }
                    else
                    {
                        LexTokens.Add(token);
                    }
                    break;
                case '"':
                    LexTokens.Add(StringLiteral());
                    break;
                case '*':
                    Advance();
                    LexTokens.Add(new Token(TokenType.Operator, "*", Line, Column));
                    break;
                case '/':
                    if (Peek() == '/') { Advance(); LexComment(); break; }
                    else
                    {
                        Advance();
                        LexTokens.Add(new Token(TokenType.Operator, "/", Line, Column));
                        break;
                    }
                case '%':
                    Advance();
                    LexTokens.Add(new Token(TokenType.Operator, "%", Line, Column));
                    break;
                case '+':
                    Advance();
                    LexTokens.Add(new Token(TokenType.Operator, "+", Line, Column));
                    break;
                case '-':
                    if (Peek() == '>')
                    {
                        Advance();
                        Advance();
                        LexTokens.Add(new Token(TokenType.LLinq, "->", Line, Column));
                    }
                    else
                    {
                        Advance();
                        LexTokens.Add(new Token(TokenType.Operator, "-", Line, Column));
                    }
                    break;
                case '(':
                    Advance();
                    LexTokens.Add(new Token(TokenType.LParen, "(", Line, Column));
                    break;
                case ')':
                    Advance();
                    LexTokens.Add(new Token(TokenType.RParen, ")", Line, Column));
                    break;
                case '{':
                    Advance();
                    LexTokens.Add(new Token(TokenType.LBrace, "{", Line, Column));
                    break;
                case '}':
                    Advance();
                    LexTokens.Add(new Token(TokenType.RBrace, "}", Line, Column));
                    break;
                case '[':
                    Advance();
                    LexTokens.Add(new Token(TokenType.LBracket, "[", Line, Column));
                    break;
                case ']':
                    Advance();
                    LexTokens.Add(new Token(TokenType.RBracket, "]", Line, Column));
                    break;
                case ';':
                    Advance();
                    LexTokens.Add(new Token(TokenType.EOL, ";", Line, Column));
                    if (CurrentChar == '\r')
                    {
                        Advance();
                        Line++;
                        Column = 1;
                    }
                    break;
                case ':':
                    Advance();
                    LexTokens.Add(new Token(TokenType.Punctuation, ":", Line, Column));
                    break;
                case ',':
                    Advance();
                    LexTokens.Add(new Token(TokenType.Comma, ",", Line, Column));
                    break;
                case '=':
                    if (Peek() == '>')
                    {
                        Advance();
                        Advance();
                        LexTokens.Add(new Token(TokenType.Flinq, "=>", Line, Column));
                    }
                    else if (Peek() == '=')
                    {
                        Advance();
                        Advance();
                        LexTokens.Add(new Token(TokenType.ComparisonOperator, "==", Line, Column));
                    }
                    else
                    {
                        Advance();
                        LexTokens.Add(new Token(TokenType.Operator, "=", Line, Column));
                    }
                    break;
                case '!':
                    if (Peek() == '=')
                    {
                        Advance();
                        Advance();
                        LexTokens.Add(new Token(TokenType.ComparisonOperator, "!=", Line, Column));
                    }
                    else
                    {
                        Advance();
                        LexTokens.Add(new Token(TokenType.Operator, "!", Line, Column));
                    }
                    break;
                case '>':
                    if (Peek() == '=')
                    {
                        Advance();
                        Advance();
                        LexTokens.Add(new Token(TokenType.ComparisonOperator, ">=", Line, Column));
                    }
                    else
                    {
                        Advance();
                        LexTokens.Add(new Token(TokenType.ComparisonOperator, ">", Line, Column));
                    }
                    break;
                case '<':
                    if (Peek() == '=')
                    {
                        Advance();
                        Advance();
                        LexTokens.Add(new Token(TokenType.ComparisonOperator, "<=", Line, Column));
                    }
                    else
                    {
                        Advance();
                        LexTokens.Add(new Token(TokenType.ComparisonOperator, "<", Line, Column));
                    }
                    break;
                case '^':
                    Advance();
                    LexTokens.Add(new Token(TokenType.Operator, "^", Line, Column));
                    break;
                case '.':
                    if (Peek() == '.' && Peek2() == '.')
                    {
                        // Consume the next two characters
                        Advance();
                        Advance();
                        Advance();
                        LexTokens.Add(new Token(TokenType.DotDotDot, "...", Line, Column));
                    }
                    else
                    {
                        Advance();
                        LexTokens.Add(new Token(TokenType.Operator, ".", Line, Column));
                    }
                    break;
                case '_':
                    Advance();
                    LexTokens.Add(new Token(TokenType.Identifier, "_", Line, Column));
                    break;
                default:
                    throw new Exception($"Invalid character {CurrentChar} at line {Line} and column {Column}");
            }
        }
        LexTokens.Add(new Token(TokenType.EOF, "", Line, Column));
    }

    private void LexComment()
    {
        while (CurrentChar != '\0' && CurrentChar != '\r')
        {
            Advance();
        }
    }
}