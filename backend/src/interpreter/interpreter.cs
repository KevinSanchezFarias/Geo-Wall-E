using LexerAnalize;
using ParserAnalize;
using EvaluatorAnalize;

namespace InterpreterAnalizer;
public class Interpreter
{
    public static object Interpret(string input)
    {
        List<ToDraw> toDraws = new();
        // Split the string into lines
        var lines = input.Split(";\r", StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            try
            {
                var lexer = new Lexer(line);
                var parser = new Parser(lexer.LexTokens);
                var evaluator = new Evaluator(parser.Parse());
                object? lineResult = evaluator.Evaluate();
                if (lineResult is not null)
                {
                    if (lineResult is ToDraw draw)
                    {
                        toDraws.Add(draw);
                    }
                    else if (lineResult is List<ToDraw> drawList)
                    {
                        toDraws.AddRange(drawList);
                    }
                }
            }
            catch (Exception e) { return e.Message; }
        }
        return toDraws;
    }
}