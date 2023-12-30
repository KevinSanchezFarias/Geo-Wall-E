using LexerAnalize;
using ParserAnalize;
using EvaluatorAnalize;
using Lists;
using System.Drawing.Text;

namespace InterpreterAnalizer;
public class Interpreter
{
    /// <summary>
    /// Interprets the input string and returns a list of objects to draw.
    /// </summary>
    /// <param name="input">The input string to interpret.</param>
    /// <returns>A list of objects to draw.</returns>
    public static IEnumerable<object> Interpret(string input)
    {
        short lineX = 1;
        List<LE.ToDraw> toDraws = new();
        // Split the string into lines
        var lines = input.Split(new[] { ";\r" }, StringSplitOptions.RemoveEmptyEntries);
        CleanFigs();
        foreach (var line in lines)
        {
            object lineResult;
            //try
            //{
            var lexer = new Lexer(line);
            var parser = new Parser(lexer.LexTokens);
            var evaluator = new Evaluator(parser.Parse());
            lineResult = evaluator.Evaluate();
            lineX++;
            //}
            //catch (Exception ex) { lineResult = $"Line:{lineX} {ex.Message}"; }
            if (lineResult is LE.ToDraw draw)
            {
                yield return draw;
            }
            else if (lineResult is List<LE.ToDraw> drawList)
            {
                toDraws.AddRange(drawList);
                yield return toDraws;
            }
            else if (lineResult is not null)
            {
                yield return lineResult;
            }
            else
            {
                continue;
            }
        }
    }
    /*Minified*/
    private static void CleanFigs() { LE.toDraws.Clear(); LE.DeclaredConst.Clear(); LE.poiND.Clear(); LE.Seqs.Clear(); LE.Color.Clear(); LE.Color.Push(Brushes.White); }
}