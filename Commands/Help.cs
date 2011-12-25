using System;
using LinkInspector.Properties;
using ManyConsole;

namespace LinkInspector.Commands
{
    public class GetHelp : ConsoleCommand
    {
        public GetHelp()
        {
            OneLineDescription = Resources.Help;
            
            
            Command = "-h";
            //OneLineDescription = "An example of how to use this application.";
        }

        public override int Run()
        {
            Console.WriteLine("Usage: LinkInspector.exe -u \"<URL>\" [options]");
            return 0;
        }
    }
}
