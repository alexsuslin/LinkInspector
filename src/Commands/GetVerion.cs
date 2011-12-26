using System.Reflection;
using LinkInspector.Properties;
using ManyConsole;
using NLog;

namespace LinkInspector.Commands
{
    internal sealed class GetVerion : ConsoleCommand
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public GetVerion()
        {
            Command = "-v";
            OneLineDescription = Resources.GetVersionRunCurrentVersionInfo;
        }

        public override int Run()
        {
            Logger.Info(Resources.GetVerionRunCurrentVersion, new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version);
            return 0;
        }
    }
}
