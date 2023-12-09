using LexerAnalize;
using ParserAnalize;
using EvaluatorAnalize;

namespace InterpreterAnalizer;
public class Interpreter
{
    public static string? Interpret(string input)
    {
        // Split the string into lines
        var lines = input.Split(";\r", StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            try
            {
                var lexer = new Lexer(line);
                var parser = new Parser(lexer.LexTokens);
                var evaluator = new Evaluator(parser.Parse());
                var lineResult = evaluator.Evaluate();

                return lineResult == null ? "" : lineResult.ToString();
            }
            catch (Exception e) { return e.Message; }
        }
        return "";
    }
}