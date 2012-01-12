using System;
using System.Text;
using ManyConsole;

namespace LinkInspector
{
    sealed class App
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var commands = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof (App));
            ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }
    }
}
