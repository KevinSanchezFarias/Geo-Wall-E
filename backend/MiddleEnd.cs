using InterpreterAnalizer;

static class MiddleEnd
{
    public static object GSharp(string input)
    {
        _ = new Interpreter();
        return Interpreter.Interpret(input)!;
    }
}