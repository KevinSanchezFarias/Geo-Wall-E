using LexerAnalize;
using ParserAnalize;
using EvaluatorAnalize;

namespace InterpreterAnalizer;
public class Interpreter
{
    public static object Interpret(string input)
    {
        // Split the string into lines
        var lines = input.Split(";\r", StringSplitOptions.RemoveEmptyEntries);

        var lineResult = default(object);
        foreach (var line in lines)
        {
            try
            {
                var lexer = new Lexer(line);
                var parser = new Parser(lexer.LexTokens);
                var evaluator = new Evaluator(parser.Parse());
                lineResult = evaluator.Evaluate();

            }
            catch (Exception e) { return e.Message; }
        }
        return lineResult ?? "";
    }
}