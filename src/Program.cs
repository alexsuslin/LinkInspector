using System;
using ManyConsole;

namespace LinkInspector
{
    sealed class App
    {
        static void Main(string[] args)
        {
            var commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof (App));
            ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }
    }
}