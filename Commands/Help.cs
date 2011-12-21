using System;
using ManyConsole;

namespace LinkInspector.Commands
{
    public class GetHelp : ConsoleCommand
    {
        public GetHelp()
        {
            Command = "-h";
            OneLineDescription = "An example of how to use this application.";
        }

        public override int Run()
        {
            Console.WriteLine("Usage: LinkInspector.exe -u \"<URL>\" [options]");
            return 0;
        }
    }
}
