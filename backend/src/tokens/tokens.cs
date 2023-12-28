namespace Tokens;
/// <summary>
/// Represents the different types of tokens in the program.
/// </summary>
public enum TokenType
{
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
    DrawKeyword,
    MeasureKeyword,
    EOL,
    EOF,
    Figure,
    ColorKeyword,
    RestoreKeyword,
    IntersectKeyword,
    DotDotDot,
    ImportKeyword
}
/// <summary>
/// Represents a token in the program.
/// </summary>
/// <param name="type">The type of the token.</param>
/// <param name="value">The value of the token.</param>
/// <param name="line">The line number where the token appears.</param>
/// <param name="column">The column number where the token appears.</param>
public class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }
    public int Line { get; set; }
    public int Column { get; set; }

    /// <summary>
    /// Represents a token in the program.
    /// </summary>
    /// <param name="type">The type of the token.</param>
    /// <param name="value">The value of the token.</param>
    /// <param name="line">The line number where the token appears.</param>
    /// <param name="column">The column number where the token appears.</param>
    public Token(TokenType type, string value, int line, int column)
    {
        Type = type;
        Value = value;
        Line = line;
        Column = column;
    }
}