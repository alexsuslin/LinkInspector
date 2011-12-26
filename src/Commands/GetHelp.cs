using System;
using LinkInspector.Properties;
using ManyConsole;

namespace LinkInspector.Commands
{
    internal sealed class GetHelp : ConsoleCommand
    {
        public GetHelp()
        {
            OneLineDescription = Resources.GetHelp_GetHelp_CommandDescription;
            Command = "-h";
            //OneLineDescription = "An example of how to use this application.";
        }

        public override int Run()
        {
            Console.WriteLine(Resources.GetHelp_Run_Usage);
            return 0;
        }
    }
}
