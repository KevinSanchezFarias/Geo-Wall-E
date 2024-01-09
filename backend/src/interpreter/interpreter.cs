using LexerAnalize;
using ParserAnalize;
using EvaluatorAnalize;
using Lists;
using WaLI.backend.src.semantic;

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
        List<LE.ToDraw> toDraws = new();
        CleanFigs();
        var lexer = new Lexer(input);
        var parser = new Parser(lexer.LexTokens);
        var ast = parser.Parse();
        /*var semanticAnalyzer = new SemanticAnalyzer();
           semanticAnalyzer.Analyze(ast); */
        var evaluator = new Evaluator(ast);
        var lineResults = evaluator.Evaluate();
        foreach (var lineResult in lineResults)
        {
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
                ;
            }
        }

    }
    /*Minified*/
    private static void CleanFigs() { LE.toDraws.Clear(); LE.DeclaredConst.Clear(); LE.poiND.Clear(); LE.Seqs.Clear(); LE.Color.Clear(); LE.Color.Push(Brushes.White); }
}