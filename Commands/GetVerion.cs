using System;
using ManyConsole;

namespace LinkInspector.Commands
{
    internal sealed class GetVerion : ConsoleCommand
    {
        public GetVerion()
        {
           
            Command = "-v";
            OneLineDescription = "Returns the current version of the application.";
        }

        public override int Run()
        {
            Console.WriteLine("Current version: 0.1");

            return 0;
        }
    }
}
