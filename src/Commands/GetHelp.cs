using System;
using LinkInspector.Properties;
using ManyConsole;

namespace LinkInspector.Commands
{
    internal sealed class GetHelp : ConsoleCommand
    {
        public GetHelp()
        {
            OneLineDescription = Resources.GetHelpCommandDescription;
            Command = "-h";
        }

        public override int Run()
        {
            Console.WriteLine(Resources.GetHelpRunUsage);
            return 0;
        }
    }
}
