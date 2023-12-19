using LexerAnalize;
using ParserAnalize;
using EvaluatorAnalize;
using Lists;

namespace InterpreterAnalizer;
public class Interpreter
{
    /// <summary>
    /// Interprets the input string and returns a list of objects to draw.
    /// </summary>
    /// <param name="input">The input string to interpret.</param>
    /// <returns>A list of objects to draw.</returns>
    public static object Interpret(string input)
    {
        List<ToDraw> toDraws = new();
        // Split the string into lines
        var lines = input.Split(new[] { ";\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        CleanFigs();
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

    /*Minified*/
    private static void CleanFigs() { LE.arcND.Clear(); LE.cirND.Clear(); LE.linND.Clear(); LE.poiND.Clear(); LE.rayND.Clear(); LE.segND.Clear(); LE.Seqs.Clear(); LE.Color.Clear(); LE.Color.Push(Brushes.Black); }
}