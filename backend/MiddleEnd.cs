using InterpreterAnalizer;

namespace WaLI.backend
{
    /// <summary>
    /// Provides functionality for executing GSharp code.
    /// </summary>
    public static class MiddleEnd
    {
        /// <summary>
        /// Executes the GSharp code and returns the result.
        /// </summary>
        /// <param name="input">The GSharp code to be executed.</param>
        /// <returns>The result of executing the GSharp code.</returns>
        public static object GSharp(string input)
        {
            _ = new Interpreter();
            return Interpreter.Interpret(input)!;
        }
    }
}