using System;
using System.Reflection;
using LinkInspector.Properties;
using ManyConsole;

namespace LinkInspector.Commands
{
    internal sealed class GetVerion : ConsoleCommand
    {
        public GetVerion()
        {
           
            Command = "-v";
            OneLineDescription = Resources.GetVersionRunCurrentVersionInfo;
        }

        public override int Run()
        {
            Console.WriteLine(Resources.GetVerionRunCurrentVersion, new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version);

            return 0;
        }
    }
}
