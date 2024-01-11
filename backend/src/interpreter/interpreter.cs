using LexerAnalize;
using ParserAnalize;
using EvaluatorAnalize;
using Lists;
using WaLI.backend.src.semantic;
using System.DirectoryServices.ActiveDirectory;

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
        ListExtrasScoper GlobalScope = new();
        List<ListExtrasScoper.ToDraw> toDraws = new();
        //CleanFigs();
        var lexer = new Lexer(input);
        var parser = new Parser(lexer.LexTokens);
        var ast = parser.Parse();
        /*var semanticAnalyzer = new SemanticAnalyzer();
           semanticAnalyzer.Analyze(ast); */
        var evaluator = new Evaluator(ast);
        var lineResults = evaluator.Evaluate(GlobalScope);
        foreach (var lineResult in lineResults)
        {
            if (lineResult is ListExtrasScoper.ToDraw draw)
            {
                yield return draw;
            }
            else if (lineResult is List<ListExtrasScoper.ToDraw> drawList)
            {
                foreach (var item in drawList)
                {
                    yield return item;
                }
                //toDraws.AddRange(drawList);
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
    //private static void CleanFigs() { ListExtrasScoper.toDraws.Clear(); ListExtrasScoper.DeclaredConst.Clear(); ListExtrasScoper.poiND.Clear(); ListExtrasScoper.Seqs.Clear(); ListExtrasScoper.Color.Clear(); ListExtrasScoper.Color.Push(Brushes.White); }
}