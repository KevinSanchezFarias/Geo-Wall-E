using LexerAnalize;
using ParserAnalize;
using EvaluatorAnalize;
using System.Text;
namespace InterpreterAnalizer;
public class Interpreter
{
    public static string Interpret(string input)
    {
        // Split the string into lines
        var lines = input.Split(";\r", StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var lexer = new Lexer(line);
            var parser = new Parser(lexer.LexTokens);
            var evaluator = new Evaluator(parser.Parse());
            var lineResult = evaluator.Evaluate();
            return lineResult.ToString()!;
        }
        return "";
    }
}