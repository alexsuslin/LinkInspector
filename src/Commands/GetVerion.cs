using System.Reflection;
using LinkInspector.Properties;
using ManyConsole;
using NLog;

namespace LinkInspector.Commands
{
    internal sealed class GetVerion : ConsoleCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public GetVerion()
        {
            Command = "-v";
            OneLineDescription = Resources.GetVersionRunCurrentVersionInfo;
        }

        public override int Run()
        {
            logger.Info(Resources.GetVerionRunCurrentVersion, new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version);
            return 0;
        }
    }
}
