using System;
using ManyConsole;

namespace LinkInspector
{
    sealed class Program
    {
        static void Main(string[] args)
        {
            
            var commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
            ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }
    }
}