namespace Tokens;
public enum TokenType
{
    Const,
    Flinq,
    LLinq,
    LParen,
    RParen,
    LBrace,
    RBrace,
    LBracket,
    RBracket,
    Operator,
    Punctuation,
    Point,
    Comma,
    ComparisonOperator,
    Identifier,
    FIdentifier,
    Number,
    Parameter,
    StringLiteral,
    LetKeyword,
    IfKeyword,
    ThenKeyword,
    ElseKeyword,
    InKeyword,
    FunctionKeyword,
    EOL,
    EOF,
    Figure,
    Color,
    Restore
}
/// <summary>
/// Represents a token in the program.
/// </summary>
/// <param name="type">The type of the token.</param>
/// <param name="value">The value of the token.</param>
/// <param name="line">The line number where the token appears.</param>
/// <param name="column">The column number where the token appears.</param>
public class Token(TokenType type, string value, int line, int column)
{
    public TokenType Type { get; set; } = type;
    public string Value { get; set; } = value;
    public int Line { get; set; } = line;
    public int Column { get; set; } = column;
}