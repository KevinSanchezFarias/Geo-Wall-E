using InterpreterAnalizer;

namespace WaLI.backend
{
    public static class MiddleEnd
    {
        public static object GSharp(string input)
        {
            _ = new Interpreter();
            return Interpreter.Interpret(input)!;
        }
    }
}